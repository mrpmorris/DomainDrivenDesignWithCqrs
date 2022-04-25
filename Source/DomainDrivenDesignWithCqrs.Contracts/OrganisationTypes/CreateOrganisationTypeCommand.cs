using MediatR;

namespace DomainDrivenDesignWithCqrs.Contracts.OrganisationTypes;

public class CreateOrganisationTypeCommand : CreateOrEditOrganisationTypeModel, IRequest<CreateOrganisationTypeResponse>
{
}
