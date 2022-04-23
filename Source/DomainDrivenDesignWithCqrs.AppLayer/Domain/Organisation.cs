namespace DomainDrivenDesignWithCqrs.AppLayer.Domain;

internal partial class Organisation : AggregateRoot
{
	public string? Name { get; set; }
}
