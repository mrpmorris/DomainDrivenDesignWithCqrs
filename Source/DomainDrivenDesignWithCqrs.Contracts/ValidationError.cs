using System.Text.Json.Serialization;

namespace DomainDrivenDesignWithCqrs.Contracts;

public readonly struct ValidationError
{
	public string Path { get; }
	public string Message { get; }

	[JsonConstructor]
	public ValidationError(string path, string message)
	{
		Path = path ?? throw new ArgumentNullException(nameof(path));
		Message = message ?? throw new ArgumentNullException(nameof(message));
	}
}
