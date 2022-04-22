namespace DomainDrivenDesignWithCqrs.Contracts;

public enum ResponseStatus
{
	Success = 0,
	UnexpectedError = 1,
	BadRequest = 2,
	ConcurrencyConflict = 3,
	UniqueIndexConflict = 4,
	Unauthorized = 5
}