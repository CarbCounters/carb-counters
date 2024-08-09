using System.Net;
using System.Text.Json.Serialization;
using CarbCounter.Application.Common.Requests;
using CarbCounter.Core.Entities;
using CarbCounter.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CarbCounter.Application.Account.Commands.HardDelete;

public record HardDeleteAccountCommand
{
    [JsonIgnore] public string Id { get; init; } = null;
}

public class HardDeleteAccountCommandHandler : IRequestHandler<HardDeleteAccountCommand>
{
    private readonly ApplicationDbContext _dbContext;

    public HardDeleteAccountCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse> Handle(HardDeleteAccountCommand request, CancellationToken cancellationToken)
    {
        AppUser? appUser =
            await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (appUser is null)
        {
            return new(HttpStatusCode.NotFound, message: $"The user with the Id {request.Id} does not exist.");
        }

        if (!appUser.IsSoftDeleted)
        {
            return new(HttpStatusCode.BadRequest,
                message: $"The user with the Id {request.Id} is not soft deleted.");
        }

        _dbContext.Remove(appUser);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new(HttpStatusCode.OK);
    }
}
