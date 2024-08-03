using System.Net;
using Microsoft.AspNetCore.Identity;
using CarbCounter.Application.Common.Requests;
using Microsoft.Extensions.Logging;
using CarbCounter.Core.Entities;
using CarbCounter.Core.Common.Interfaces;
using CarbCounter.Core.Enums;
using CarbCounter.Infrastructure.Identity;

namespace CarbCounter.Application.Account.Commands.Create;

public class CreateAccountCommand
{
    public required string Forename { get; init; }
    public required string Surname { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required string PhoneNumber { get; init; }
    public required string Password { get; init; }
}

public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, string>
{
    private readonly static Dictionary<EnUserType, string> s_roleMappings = new()
    {
        { EnUserType.User, UserRoles.User },
        { EnUserType.Administrator, UserRoles.Administrator },
        { EnUserType.Developer, UserRoles.Developer }
    };

    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<CreateAccountCommandHandler> _logger;
    private readonly IDateTimeService _dateTimeService;

    public CreateAccountCommandHandler(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager, 
        ILogger<CreateAccountCommandHandler> logger,
        IDateTimeService dateTimeService)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _logger = logger;
        _dateTimeService = dateTimeService;
    }

    public async Task<AppResponse<string>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        AppUser? existingUser = await _userManager.FindByNameAsync(request.Username) ?? await _userManager.FindByEmailAsync(request.Email);   

        if (existingUser is not null)
        {
            return new(HttpStatusCode.BadRequest, "User already exists."); // TODO: Make this a generic error
        }

        AppUser user = new()
        {
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            UserName = request.Username,
            Forename = request.Forename,
            Surname = request.Surname,
            SecurityStamp = Guid.NewGuid().ToString(),
            CreatedAt = _dateTimeService.Now,
            UserType = EnUserType.User,
            RegistrationStatus = EnRegistrationStatus.AccountCreated,
        };

        IdentityResult creationResult = await _userManager.CreateAsync(user, request.Password);

        if (!creationResult.Succeeded)
        {
            _logger.LogError("Unable to create a user because:");

            foreach(IdentityError error in creationResult.Errors)
            {
                _logger.LogError(error.Description);
            }

            return new(HttpStatusCode.BadRequest, message: "Unable to create account.");
        }

        if (!await _roleManager.RoleExistsAsync(s_roleMappings[user.UserType]))
        {
            IdentityResult newRoleResult = await _roleManager.CreateAsync(new IdentityRole(s_roleMappings[user.UserType]));
        
            if (!newRoleResult.Succeeded)
            {
                return new(HttpStatusCode.InternalServerError, message: "Something went wrong creating a role.");
            }
        }

        await _userManager.AddToRoleAsync(user, s_roleMappings[user.UserType]);

        return new(HttpStatusCode.OK, user.Id);
    }
}
