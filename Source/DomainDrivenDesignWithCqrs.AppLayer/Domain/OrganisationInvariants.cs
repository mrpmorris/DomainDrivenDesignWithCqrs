using FluentValidation;

namespace DomainDrivenDesignWithCqrs.AppLayer.Domain;

internal class OrganisationInvariants : AbstractValidator<Organisation>
{
	public OrganisationInvariants()
	{
		RuleFor(x => x.Name).NotEmpty();
	}
}
