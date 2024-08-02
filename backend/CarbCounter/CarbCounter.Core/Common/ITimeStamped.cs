namespace CarbCounter.Core.Common;

public interface ITimeStamped
{
    public DateTime CreatedAt { get; }
    public DateTime? UpdatedAt { get; }
}
