using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;

internal partial class Organisation : AggregateRoot
{
	[Required, MaxLength(100)]
	public string Name { get; set; } = "";
	public Guid TypeId { get; set; }

	internal class Invariants : AbstractValidator<Organisation>
	{
		public Invariants()
		{
			RuleFor(x => x.TypeId).NotEqual(Guid.Empty).WithMessage("Required");
		}
	}
}
