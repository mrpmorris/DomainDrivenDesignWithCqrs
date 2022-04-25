using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainDrivenDesignWithCqrs.AppLayer.Persistence.Repositories;
using DomainDrivenDesignWithCqrs.AppLayer.Services;
using DomainDrivenDesignWithCqrs.Contracts;
using DomainDrivenDesignWithCqrs.Contracts.Organisations;
using MediatR;

namespace DomainDrivenDesignWithCqrs.AppLayer.Cqrs.Organisations;

internal class OrganisationSearchQueryHandler : IRequestHandler<OrganisationSearchQuery, OrganisationSearchResponse>
{
	private readonly IMapper Mapper;
	private readonly IOrganisationRepository Repository;
	private readonly ISearchService Search;

	public OrganisationSearchQueryHandler(
		IMapper mapper,
		IOrganisationRepository repository,
		ISearchService search)
	{
		Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		Repository = repository ?? throw new ArgumentNullException(nameof(repository));
		Search = search ?? throw new ArgumentNullException(nameof(search));
	}

	public async Task<OrganisationSearchResponse> Handle(OrganisationSearchQuery request, CancellationToken cancellationToken)
	{
		IQueryable<OrganisationSearchItemModel> source =
			Repository.Query().ProjectTo<OrganisationSearchItemModel>(Mapper.ConfigurationProvider);

		PagedItemsModel<OrganisationSearchItemModel> result =
			await Search.SearchAsync(
				pageNumber: request.PageNumber,
				pageSize: request.PageSize,
				source: Repository.Query().ProjectTo<OrganisationSearchItemModel>(Mapper.ConfigurationProvider),
				addFilter:
					source =>
						string.IsNullOrWhiteSpace(request.SearchPhrase)
						? source
						: source.Where(x => x.Name.StartsWith(request.SearchPhrase)),
				addOrdering: source => source.OrderBy(x => x.Name).ThenBy(x => x.Id));

		return new OrganisationSearchResponse(result);
	}
}
