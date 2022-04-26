using FluentValidation;

namespace DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;

internal partial class Organisation : AggregateRoot
{
	public string Name { get; set; } = "";
	public Guid TypeId { get; set; }

	internal class Invariants : AbstractValidator<Organisation>
	{
		public Invariants()
		{
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.TypeId).NotEqual(Guid.Empty).WithMessage("Required");
		}
	}
}
