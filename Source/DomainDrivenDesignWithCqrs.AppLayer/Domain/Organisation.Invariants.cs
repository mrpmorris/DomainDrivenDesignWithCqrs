using FluentValidation;

namespace DomainDrivenDesignWithCqrs.AppLayer.Domain;

internal partial class Organisation
{
	private class OrganisationInvariants : AbstractValidator<Organisation>
	{
		public OrganisationInvariants()
		{
			RuleFor(x => x.Name).NotEmpty();
		}
	}
}
