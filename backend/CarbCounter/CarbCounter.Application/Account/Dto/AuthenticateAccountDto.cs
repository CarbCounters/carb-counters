namespace CarbCounter.Application.Account.Dto;

public record AuthenticateAccountDto
{
    public required string Token { get; init; }
    public required DateTime ValidTo { get; init; }
}