using System.Text.Json.Serialization;

namespace DomainDrivenDesignWithCqrs.Contracts.OrganisationTypes
{
	public class CreateOrganisationTypeResponse : ResponseBase
	{
		public Guid OrganisationTypeId { get; private set; }

		public CreateOrganisationTypeResponse() { }

		[JsonConstructor]
		public CreateOrganisationTypeResponse(Guid organisationTypeId)
		{
			if (organisationTypeId == Guid.Empty)
				throw new ArgumentException(paramName: nameof(organisationTypeId), message: "Cannot be empty");
			OrganisationTypeId = organisationTypeId;
		}
	}
}