using FluentValidation;

namespace DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;

internal class OrganisationType : AggregateRoot
{
	public string Name { get; set; } = "";

	internal class Invariants : AbstractValidator<OrganisationType>
	{
		public Invariants()
		{
			RuleFor(x => x.Name).NotEmpty();
		}
	}
}
