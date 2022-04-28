using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainDrivenDesignWithCqrs.AppLayer.Persistence.Repositories;
using DomainDrivenDesignWithCqrs.AppLayer.Services;
using DomainDrivenDesignWithCqrs.Contracts;
using DomainDrivenDesignWithCqrs.Contracts.OrganisationTypes;
using MediatR;

namespace DomainDrivenDesignWithCqrs.AppLayer.RequestHandlers.OrganisationTypes;

internal class OrganisationTypeSearchQueryHandler : IRequestHandler<OrganisationTypeSearchQuery, OrganisationTypeSearchResponse>
{
	private readonly IMapper Mapper;
	private readonly IOrganisationTypeRepository Repository;
	private readonly ISearchService Search;

	public OrganisationTypeSearchQueryHandler(
		IMapper mapper,
		IOrganisationTypeRepository repository,
		ISearchService search)
	{
		Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		Repository = repository ?? throw new ArgumentNullException(nameof(repository));
		Search = search ?? throw new ArgumentNullException(nameof(search));
	}

	public async Task<OrganisationTypeSearchResponse> Handle(OrganisationTypeSearchQuery request, CancellationToken cancellationToken)
	{
		IQueryable<OrganisationTypeSearchItemModel> source =
			Repository
				.Query()
				.Where(x => 
					string.IsNullOrWhiteSpace(request.SearchPhrase)
					|| x.Name.Contains(request.SearchPhrase))
				.OrderBy(x => x.Name).ThenBy(x => x.Id)
				.ProjectTo<OrganisationTypeSearchItemModel>(Mapper.ConfigurationProvider);

		PagedItemsModel<OrganisationTypeSearchItemModel> result = await
			Search.SearchAsync(
				source: source,
				pageNumber: request.PageNumber,
				itemsPerPage: request.ItemsPerPage,
				filter:
					source =>
						string.IsNullOrWhiteSpace(request.SearchPhrase)
						? source
						: source.Where(x => x.Name.Contains(request.SearchPhrase)),
				sort: source => source.OrderBy(x => x.Name).ThenBy(x => x.Id));

		return new OrganisationTypeSearchResponse(result);
	}
}
