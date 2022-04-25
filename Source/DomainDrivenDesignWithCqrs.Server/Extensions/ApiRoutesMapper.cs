using DomainDrivenDesignWithCqrs.Contracts.Organisations;
using DomainDrivenDesignWithCqrs.Contracts.OrganisationTypes;

namespace DomainDrivenDesignWithCqrs.Server.Extensions;

public static class ApiRoutesMapper
{
	public static IEndpointRouteBuilder MapApplicationApiEndpoints(this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapApiPost<CreateOrganisationCommand, CreateOrganisationResponse>("/api/organisation/create");
		endpoints.MapApiPost<OrganisationSearchQuery, OrganisationSearchResponse>("/api/organisation/search");
		endpoints.MapApiPost<CreateOrganisationTypeCommand, CreateOrganisationTypeResponse>("/api/organisation-type/create");
		endpoints.MapApiPost<OrganisationTypeSearchQuery, OrganisationTypeSearchResponse>("/api/organisation-type/search");
		return endpoints;
	}
}
