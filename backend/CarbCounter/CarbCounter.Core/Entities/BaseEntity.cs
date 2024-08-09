namespace CarbCounter.Core.Entities;

public class BaseEntity
{
    public required int Id { get; init; }
    public bool IsSoftDeleted { get; set; } = false;
    public bool IsAudited { get; set; } = false;
}