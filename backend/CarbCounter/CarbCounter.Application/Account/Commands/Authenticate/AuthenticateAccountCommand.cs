using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using CarbCounter.Application.Account.Dto;
using CarbCounter.Application.Common.Requests;
using CarbCounter.Core.Common.Interfaces;
using CarbCounter.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace CarbCounter.Application.Account.Commands.Authenticate;

public record AuthenticateAccountCommand
{
    public required string Username { get; init; }
    public required string Password { get; init; }
}

public class AuthenticateAccountCommandHandler : IRequestHandler<AuthenticateAccountCommand, AuthenticateAccountDto>
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IJsonWebTokenService _jsonWebTokenService;
    private readonly ILogger<AuthenticateAccountCommandHandler> _logger;
    private readonly IDateTimeService _dateTimeService;

    public AuthenticateAccountCommandHandler(
        UserManager<AppUser> userManager, 
        IJsonWebTokenService jsonWebTokenService, 
        ILogger<AuthenticateAccountCommandHandler> logger, 
        IDateTimeService dateTimeService)
    {
        _userManager = userManager;
        _jsonWebTokenService = jsonWebTokenService;
        _logger = logger;
        _dateTimeService = dateTimeService;
    }

    public async Task<AppResponse<AuthenticateAccountDto>> Handle(AuthenticateAccountCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await _userManager.FindByNameAsync(request.Username) ??
                       await _userManager.FindByEmailAsync(request.Username);
        
        if (user is null)
        {
            return new(HttpStatusCode.Unauthorized, message: "User does not exist.");
        }

        bool passwordIsCorrect = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!passwordIsCorrect)
        {
            return new(HttpStatusCode.Unauthorized, message: "Bad password");
        }

        List<Claim> claims = new()
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.GivenName, user.Forename),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        foreach (var role in await _userManager.GetRolesAsync(user))
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        JwtSecurityToken securityToken = _jsonWebTokenService.GetToken(claims);

        string token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        user.LastLogon = _dateTimeService.Now;
        await _userManager.UpdateAsync(user);

        _logger.LogInformation("User logged in as: {username}", user.UserName);
        
        AuthenticateAccountDto dto = new()
        {
            Token = token,
            ValidTo = securityToken.ValidTo
        };

        return new(HttpStatusCode.OK, dto);
    }
}