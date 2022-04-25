namespace DomainDrivenDesignWithCqrs.Contracts.OrganisationTypes;

public class OrganisationTypeSearchResponse : PagedItemsResponseBase<OrganisationTypeSearchItemModel>
{
	public OrganisationTypeSearchResponse() { }

	public OrganisationTypeSearchResponse(PagedItemsModel<OrganisationTypeSearchItemModel> result) : base(result) { }
}
