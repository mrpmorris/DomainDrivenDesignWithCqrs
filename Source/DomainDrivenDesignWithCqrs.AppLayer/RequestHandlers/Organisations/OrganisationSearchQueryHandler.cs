using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;
using DomainDrivenDesignWithCqrs.AppLayer.Persistence.Repositories;
using DomainDrivenDesignWithCqrs.AppLayer.Persistence.ViewSources;
using DomainDrivenDesignWithCqrs.AppLayer.Services;
using DomainDrivenDesignWithCqrs.Contracts;
using DomainDrivenDesignWithCqrs.Contracts.Organisations;
using MediatR;

namespace DomainDrivenDesignWithCqrs.AppLayer.RequestHandlers.Organisations;

internal class OrganisationSearchQueryHandler : IRequestHandler<OrganisationSearchQuery, OrganisationSearchResponse>
{
	private readonly IOrganisationSearchItemModelViewSource ViewSource;
	private readonly ISearchService Search;

	public OrganisationSearchQueryHandler(
		IOrganisationSearchItemModelViewSource viewSource,
		ISearchService search)
	{
		ViewSource = viewSource ?? throw new ArgumentNullException(nameof(viewSource));
		Search = search ?? throw new ArgumentNullException(nameof(search));
	}

	public async Task<OrganisationSearchResponse> Handle(OrganisationSearchQuery request, CancellationToken cancellationToken)
	{
		PagedItemsModel<OrganisationSearchItemModel> result =
			await Search.SearchAsync(
				source: ViewSource.Views,
				pageNumber: request.PageNumber,
				itemsPerPage: request.ItemsPerPage,
				filter: x =>
					string.IsNullOrWhiteSpace(request.SearchPhrase)
					? x
					: x.Where(x =>
						x.Name.Contains(request.SearchPhrase)
						|| x.Type.Contains(request.SearchPhrase)),
				sort: x => x.OrderBy(x => x.Name).ThenBy(x => x.Id));

		return new OrganisationSearchResponse(result);
	}
}
