namespace CarbCounter.Core.Common;

public interface IAuditStamped
{
    public string CreatedAtById { get; }
    public string CreatedAtByName { get; }

    public string UpdatedAtById { get; }
    public string UpdatedAtByName { get; }
}