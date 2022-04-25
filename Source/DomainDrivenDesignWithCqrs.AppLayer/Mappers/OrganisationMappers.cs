using AutoMapper;
using DomainDrivenDesignWithCqrs.AppLayer.Domain;
using DomainDrivenDesignWithCqrs.Contracts.Organisation;

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
