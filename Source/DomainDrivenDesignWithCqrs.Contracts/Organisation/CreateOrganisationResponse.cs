namespace DomainDrivenDesignWithCqrs.Contracts.Organisation
{
	public class CreateOrganisationResponse : ResponseBase
	{
		public Guid OrganisationId { get; set; }

		public CreateOrganisationResponse() { }

		public CreateOrganisationResponse(Guid organisationId)
		{
			if (organisationId == Guid.Empty)
				throw new ArgumentException(paramName: nameof(organisationId), message: "Cannot be empty");
			OrganisationId = organisationId;
		}
	}
}