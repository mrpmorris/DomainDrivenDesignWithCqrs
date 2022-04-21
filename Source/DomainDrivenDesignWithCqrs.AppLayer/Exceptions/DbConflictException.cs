namespace DomainDrivenDesignWithCqrs.AppLayer.Exceptions;

public class DbConflictException : Exception
{
	public DbConflictException()
	{
	}

	public DbConflictException(string message) : base(message)
	{
	}

	public DbConflictException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
