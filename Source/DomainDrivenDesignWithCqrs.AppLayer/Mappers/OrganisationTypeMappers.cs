﻿using AutoMapper;
using DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;
using DomainDrivenDesignWithCqrs.Contracts.OrganisationTypes;

namespace DomainDrivenDesignWithCqrs.AppLayer.Mappers;

internal class OrganisationTypeMappers : Profile
{
	public OrganisationTypeMappers()
	{
		CreateMap<OrganisationType, CreateOrEditOrganisationTypeModel>()
			.ReverseMap();
		CreateMap<OrganisationType, OrganisationTypeSearchItemModel>();
	}
}
