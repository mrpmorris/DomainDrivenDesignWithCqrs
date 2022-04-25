using System.Text.Json.Serialization;

namespace DomainDrivenDesignWithCqrs.Contracts.Organisations
{
	public class CreateOrganisationResponse : ResponseBase
	{
		public Guid OrganisationId { get; private set; }

		public CreateOrganisationResponse() { }

		[JsonConstructor]
		public CreateOrganisationResponse(Guid organisationId)
		{
			if (organisationId == Guid.Empty)
				throw new ArgumentException(paramName: nameof(organisationId), message: "Cannot be empty");
			OrganisationId = organisationId;
		}
	}
}