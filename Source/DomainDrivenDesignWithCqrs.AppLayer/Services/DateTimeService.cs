namespace DomainDrivenDesignWithCqrs.AppLayer.Services;

public class IDateTimeService
{
	DateTime UtcNow { get; }
	DateTime UtcToday { get; }
}

internal class DateTimeService : IDateTimeService
{
	public DateTime UtcNow => DateTime.UtcNow;
	public DateTime UtcToday => DateTime.UtcNow.Date;
}
