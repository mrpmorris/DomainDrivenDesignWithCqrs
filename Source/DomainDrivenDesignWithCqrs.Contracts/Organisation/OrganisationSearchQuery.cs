using FluentValidation;
using MediatR;

namespace DomainDrivenDesignWithCqrs.Contracts.Organisation;

public class OrganisationSearchQuery : IRequest<OrganisationSearchResponse>
{
	public int PageNumber { get; set; }
	public int PageSize { get; set; }
	public string? SearchPhrase { get; set; }

	internal class Validator : AbstractValidator<OrganisationSearchQuery>
	{
		public Validator()
		{
			RuleFor(x => x.PageNumber).GreaterThan(0);
			RuleFor(x => x.PageSize).GreaterThan(0);
		}
	}
}
