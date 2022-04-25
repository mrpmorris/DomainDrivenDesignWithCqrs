using System.Text.Json.Serialization;

namespace DomainDrivenDesignWithCqrs.Contracts.OrganisationTypes;

public class OrganisationTypeSearchItemModel
{
	public Guid Id { get; private set; }
	public string Name { get; private set; } = "";

	[JsonConstructor]
	public OrganisationTypeSearchItemModel(Guid id, string? name)
	{
		ArgumentNullException.ThrowIfNull(name);

		Id = id;
		Name = name;
	}
}
