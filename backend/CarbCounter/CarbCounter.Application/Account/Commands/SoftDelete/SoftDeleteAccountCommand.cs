﻿using System.Net;
using System.Text.Json.Serialization;
using CarbCounter.Application.Common.Requests;
using CarbCounter.Core.Entities;
using CarbCounter.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CarbCounter.Application.Account.Commands.SoftDelete;

public record SoftDeleteAccountCommand
{
    [JsonIgnore] public string Id { get; init; } = null!;
}

public class SoftDeleteAccountCommandHandler : IRequestHandler<SoftDeleteAccountCommand>
{
    private readonly ApplicationDbContext _dbContext;

    public SoftDeleteAccountCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AppResponse> Handle(SoftDeleteAccountCommand request, CancellationToken cancellationToken)
    {
        AppUser? appUser =
            await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken: cancellationToken);

        if (appUser is null)
        {
            return new(HttpStatusCode.NotFound, message: $"The user with the Id {request.Id} does not exist.");
        }

        if (appUser.IsSoftDeleted)
        {
            return new(HttpStatusCode.BadRequest,
                message: $"The user with the Id {request.Id} is already soft deleted.");
        }

        appUser.IsSoftDeleted = true;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new(HttpStatusCode.OK);
    }
}