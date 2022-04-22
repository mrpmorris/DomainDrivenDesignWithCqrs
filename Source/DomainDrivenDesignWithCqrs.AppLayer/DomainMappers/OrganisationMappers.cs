using AutoMapper;
using DomainDrivenDesignWithCqrs.AppLayer.Domain;
using DomainDrivenDesignWithCqrs.Contracts.Organisation;

namespace DomainDrivenDesignWithCqrs.AppLayer.DomainMappers;

internal class OrganisationMappers : Profile
{
	public OrganisationMappers()
	{
		CreateMap<CreateOrEditOrganisationModel, Organisation>()
			.ReverseMap();
	}
}
