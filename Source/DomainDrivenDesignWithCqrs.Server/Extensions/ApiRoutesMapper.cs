using DomainDrivenDesignWithCqrs.Contracts.Organisations;

namespace DomainDrivenDesignWithCqrs.Server.Extensions;

public static class ApiRoutesMapper
{
	public static IEndpointRouteBuilder MapApplicationApiEndpoints(this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapApiPost<CreateOrganisationCommand, CreateOrganisationResponse>("/api/organisation/create");
		endpoints.MapApiPost<OrganisationSearchQuery, OrganisationSearchResponse>("/api/organisation/search");
		return endpoints;
	}
}
