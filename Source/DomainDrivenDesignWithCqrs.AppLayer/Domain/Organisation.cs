using FluentValidation;

namespace DomainDrivenDesignWithCqrs.AppLayer.Domain;

internal partial class Organisation : AggregateRoot
{
	public string Name { get; set; } = "";
	public Guid Type { get; set; }

	internal class Invariants : AbstractValidator<Organisation>
	{
		public Invariants()
		{
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.Type).NotEqual(Guid.Empty).WithMessage("Required");
		}
	}
}
