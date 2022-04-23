using DomainDrivenDesignWithCqrs.Contracts.Organisation;

namespace DomainDrivenDesignWithCqrs.Server.Extensions;

public static class ApiRoutesMapper
{
	public static IEndpointRouteBuilder MapApplicationApiEndpoints(this IEndpointRouteBuilder endpoints)
	{
		endpoints.MapApiPost<CreateOrganisationCommand, CreateOrganisationResponse>("/api/organisation/create");
		return endpoints;
	}
}
