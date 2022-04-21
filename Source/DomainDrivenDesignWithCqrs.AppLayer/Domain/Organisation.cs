namespace DomainDrivenDesignWithCqrs.AppLayer.Domain;

internal class Organisation : AggregateRoot
{
	public string? Name { get; set; }
}
