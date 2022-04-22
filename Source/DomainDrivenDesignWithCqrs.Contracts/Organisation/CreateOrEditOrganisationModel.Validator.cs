using FluentValidation;

namespace DomainDrivenDesignWithCqrs.Contracts.Organisation;

public class CreateOrEditOrganisationModelValidator : AbstractValidator<CreateOrEditOrganisationModel>
{
	public CreateOrEditOrganisationModelValidator()
	{
		RuleFor(x => x.Name).NotEmpty();
	}
}
