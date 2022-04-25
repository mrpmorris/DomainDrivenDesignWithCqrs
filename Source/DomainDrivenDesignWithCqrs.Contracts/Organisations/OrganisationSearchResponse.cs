namespace DomainDrivenDesignWithCqrs.Contracts.Organisations;

public class OrganisationSearchResponse : PagedItemsResponseBase<OrganisationSearchItemModel>
{
	public OrganisationSearchResponse() { }

	public OrganisationSearchResponse(PagedItemsModel<OrganisationSearchItemModel> result) : base(result) { }
}
