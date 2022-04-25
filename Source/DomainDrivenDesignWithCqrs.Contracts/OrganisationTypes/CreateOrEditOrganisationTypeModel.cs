using FluentValidation;

namespace DomainDrivenDesignWithCqrs.Contracts.OrganisationTypes;

public class CreateOrEditOrganisationTypeModel
{
	public string? Name { get; set; }

	internal class Validator : AbstractValidator<CreateOrEditOrganisationTypeModel>
	{
		public Validator()
		{
			RuleFor(x => x.Name).NotEmpty();
		}
	}
}
