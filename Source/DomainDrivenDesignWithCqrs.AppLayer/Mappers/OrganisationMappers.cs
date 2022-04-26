using AutoMapper;
using DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;
using DomainDrivenDesignWithCqrs.Contracts.Organisations;

namespace DomainDrivenDesignWithCqrs.AppLayer.Mappers;

internal class OrganisationMappers : Profile
{
	public OrganisationMappers()
	{
		CreateMap<Organisation, CreateOrEditOrganisationModel>()
			.ReverseMap();
		CreateMap<Organisation, OrganisationSearchItemModel>();
	}
}
