﻿using FluentValidation;
using MediatR;

namespace DomainDrivenDesignWithCqrs.Contracts.OrganisationTypes;

public class OrganisationTypeSearchQuery : IRequest<OrganisationTypeSearchResponse>
{
	public int PageNumber { get; set; }
	public int ItemsPerPage { get; set; }
	public string? SearchPhrase { get; set; }

	internal class Validator : AbstractValidator<OrganisationTypeSearchQuery>
	{
		public Validator()
		{
			RuleFor(x => x.PageNumber).GreaterThan(0);
			RuleFor(x => x.ItemsPerPage).GreaterThan(0);
		}
	}
}
