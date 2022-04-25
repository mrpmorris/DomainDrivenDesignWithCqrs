namespace DomainDrivenDesignWithCqrs.Contracts.Organisation;

public class OrganisationSearchItemModel
{
	public Guid Id { get; set; }
	public string Name { get; set; } = "";

	public OrganisationSearchItemModel() { }

	public OrganisationSearchItemModel(Guid id, string? name)
	{
		ArgumentNullException.ThrowIfNull(name);

		Id = id;
		Name = name;
	}
}
