namespace DomainDrivenDesignWithCqrs.Contracts;

public enum ResponseStatus
{
	Success = 0,
	UnexpectedError = 1,
	BadRequest = 2,
	ConcurrencyConflict = 3,
	Unauthorized = 4
}