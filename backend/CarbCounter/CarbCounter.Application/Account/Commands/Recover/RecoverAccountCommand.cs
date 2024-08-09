using System.Net;
using CarbCounter.Application.Common.Requests;
using CarbCounter.Core.Entities;
using CarbCounter.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace CarbCounter.Application.Account.Commands.Recover;

public record RecoverAccountCommand
{
    [JsonIgnore] public string Id { get; init; } = null!;
}

public class RecoverAccountCommandHandler : IRequestHandler<RecoverAccountCommand>
{
    private readonly ApplicationDbContext _dbContext;

    public RecoverAccountCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse> Handle(RecoverAccountCommand request, CancellationToken cancellationToken)
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

        appUser.IsSoftDeleted = false;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new(HttpStatusCode.OK);
    }
}