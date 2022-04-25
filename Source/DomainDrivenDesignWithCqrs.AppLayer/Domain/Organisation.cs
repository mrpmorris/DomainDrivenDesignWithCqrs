using FluentValidation;

namespace DomainDrivenDesignWithCqrs.AppLayer.Domain;

internal partial class Organisation : AggregateRoot
{
	public string Name { get; set; } = "";

	internal class Invariants : AbstractValidator<Organisation>
	{
		public Invariants()
		{
			RuleFor(x => x.Name).NotEmpty();
		}
	}
}
