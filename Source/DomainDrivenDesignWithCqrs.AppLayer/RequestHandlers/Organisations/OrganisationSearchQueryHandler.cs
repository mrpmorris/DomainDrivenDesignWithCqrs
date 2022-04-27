using AutoMapper;
using AutoMapper.QueryableExtensions;
using DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;
using DomainDrivenDesignWithCqrs.AppLayer.Persistence.Repositories;
using DomainDrivenDesignWithCqrs.AppLayer.Services;
using DomainDrivenDesignWithCqrs.Contracts;
using DomainDrivenDesignWithCqrs.Contracts.Organisations;
using MediatR;

namespace DomainDrivenDesignWithCqrs.AppLayer.RequestHandlers.Organisations;

internal class OrganisationSearchQueryHandler : IRequestHandler<OrganisationSearchQuery, OrganisationSearchResponse>
{
	private readonly IMapper Mapper;
	private readonly IOrganisationRepository OrganisationRepository;
	private readonly IOrganisationTypeRepository OrganisationTypeRepository;
	private readonly ISearchService Search;

	public OrganisationSearchQueryHandler(
		IMapper mapper,
		IOrganisationRepository organisationRepository,
		IOrganisationTypeRepository organisationTypeRepository,
		ISearchService search)
	{
		Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		OrganisationRepository = organisationRepository ?? throw new ArgumentNullException(nameof(organisationRepository));
        OrganisationTypeRepository = organisationTypeRepository ?? throw new ArgumentNullException(nameof(organisationTypeRepository));
        Search = search ?? throw new ArgumentNullException(nameof(search));
	}

	public async Task<OrganisationSearchResponse> Handle(OrganisationSearchQuery request, CancellationToken cancellationToken)
	{
		IQueryable<OrganisationSearchItemModel> source =
			(
				from o in OrganisationRepository.Query()
				join t in OrganisationTypeRepository.Query()
					on o.TypeId equals t.Id
				orderby o.Name, o.Id
				where
					string.IsNullOrWhiteSpace(request.SearchPhrase)
					|| o.Name.Contains(request.SearchPhrase)
					|| t.Name.Contains(request.SearchPhrase)
				select new Tuple<Organisation, OrganisationType>(o, t)
			)
			.ProjectTo<OrganisationSearchItemModel>(Mapper.ConfigurationProvider);
		//OrganisationRepository
		//	.Query()
		//	.Where(x => 
		//		string.IsNullOrWhiteSpace(request.SearchPhrase)
		//		|| x.Name.Contains(request.SearchPhrase))
		//	.ProjectTo<OrganisationSearchItemModel>(Mapper.ConfigurationProvider);

		PagedItemsModel < OrganisationSearchItemModel > result =
			await Search.SearchAsync(
				pageNumber: request.PageNumber,
				itemsPerPage: request.ItemsPerPage,
				source: source);

		return new OrganisationSearchResponse(result);
	}
}
