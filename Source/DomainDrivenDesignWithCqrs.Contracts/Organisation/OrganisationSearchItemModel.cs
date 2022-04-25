using System.Text.Json.Serialization;

namespace DomainDrivenDesignWithCqrs.Contracts.Organisation;

public class OrganisationSearchItemModel
{
	public Guid Id { get; private set; }
	public string Name { get; private set; } = "";

	[JsonConstructor]
	public OrganisationSearchItemModel(Guid id, string? name)
	{
		ArgumentNullException.ThrowIfNull(name);

		Id = id;
		Name = name;
	}
}
