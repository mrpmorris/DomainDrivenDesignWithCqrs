using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;

internal class OrganisationType : AggregateRoot
{
	[Required, MaxLength(100)]
	public string Name { get; set; } = "";

	internal class Invariants : AbstractValidator<OrganisationType>
	{
		public Invariants()
		{
		}
	}
}
