using MediatR;

namespace DomainDrivenDesignWithCqrs.Contracts.Organisations;

public class CreateOrganisationCommand : CreateOrEditOrganisationModel, IRequest<CreateOrganisationResponse>
{
}
