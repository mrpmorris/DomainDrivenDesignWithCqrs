using FluentValidation;

namespace DomainDrivenDesignWithCqrs.Contracts.Organisations;

public class CreateOrEditOrganisationModel
{
	public string? Name { get; set; }
	public Guid OrganisationTypeId { get; set; }

	internal class Validator : AbstractValidator<CreateOrEditOrganisationModel>
	{
		public Validator()
		{
			RuleFor(x => x.Name).NotEmpty();
			RuleFor(x => x.OrganisationTypeId).NotEqual(Guid.Empty).WithMessage("Required");
		}
	}
}
