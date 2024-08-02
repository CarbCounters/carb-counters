using Microsoft.AspNetCore.Identity;
using CarbCounter.Core.Common;
using CarbCounter.Core.Enums;

namespace CarbCounter.Core.Entities;

public class AppUser : IdentityUser, ITimeStamped
{
    public required string Forename { get; set; } = null!;
    public required string Surname { get; set; } = null!;
    public required DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; set; }
    public required EnUserType UserType { get; set; } = EnUserType.User;
    public int UserTypeId => (int)UserType;
}