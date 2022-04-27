namespace DomainDrivenDesignWithCqrs.Contracts.Organisations;

public class OrganisationSearchItemModel
{
	public Guid Id { get; private set; }
	public string Name { get; private set; } = "";
	public string Type { get; private set; } = "";
}
