namespace CarbCounter.Application.Account.Commands.SoftDelete;

public record SoftDeleteAccountCommand
{
    public required string Id { get; init; }
}

