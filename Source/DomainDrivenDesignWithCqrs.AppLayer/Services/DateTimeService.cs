namespace DomainDrivenDesignWithCqrs.AppLayer.Services;

internal class IDateTimeService
{
	DateTime UtcNow { get; }
	DateTime UtcToday { get; }
}

internal class DateTimeService : IDateTimeService
{
	public static DateTime UtcNow => DateTime.UtcNow;
	public static DateTime UtcToday => DateTime.UtcNow.Date;
}
