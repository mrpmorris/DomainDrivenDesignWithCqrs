using AutoMapper;
using DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;
using DomainDrivenDesignWithCqrs.Contracts.Organisations;

namespace DomainDrivenDesignWithCqrs.AppLayer.ContractMappers;

internal class OrganisationMappers : Profile
{
	public OrganisationMappers()
	{
		CreateMap<Organisation, CreateOrEditOrganisationModel>()
			.ReverseMap();
		CreateMap<Organisation, OrganisationSearchItemModel>();
		CreateMap<Tuple<Organisation, OrganisationType>, OrganisationSearchItemModel>()
			.ForMember(x => x.Id, x => x.MapFrom(x => x.Item1.Id))
			.ForMember(x => x.Name, x => x.MapFrom(x => x.Item1.Name))
			.ForMember(x => x.Type, x => x.MapFrom(x => x.Item2.Name));
	}
}
