namespace DomainDrivenDesignWithCqrs.Contracts;

public class ResponseBase
{
	public ResponseStatus Status { get; set; } = ResponseStatus.Success;
	public string? ErrorMessage { get; set; }
	public IEnumerable<ValidationError>? ValidationErrors { get; set; }
}
