using CarbCounter.Core.Common.Interfaces;

namespace CarbCounter.Core.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.Now;
}
