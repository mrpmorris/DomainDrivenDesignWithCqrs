using AutoMapper;
using DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;
using DomainDrivenDesignWithCqrs.Contracts.OrganisationTypes;

namespace DomainDrivenDesignWithCqrs.AppLayer.ContractMappers;

internal class OrganisationTypeMappers : Profile
{
	public OrganisationTypeMappers()
	{
		CreateMap<OrganisationType, CreateOrEditOrganisationTypeModel>()
			.ReverseMap();
	}
}
