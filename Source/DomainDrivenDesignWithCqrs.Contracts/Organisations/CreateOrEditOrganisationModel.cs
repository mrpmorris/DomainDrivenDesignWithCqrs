using FluentValidation;

namespace DomainDrivenDesignWithCqrs.Contracts.Organisations;

public class CreateOrEditOrganisationModel
{
	public string Name { get; set; } = "";
	public Guid TypeId { get; set; }

	internal class Validator : AbstractValidator<CreateOrEditOrganisationModel>
	{
		public Validator()
		{
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.TypeId).NotEqual(Guid.Empty).WithMessage("Required");
		}
	}
}
