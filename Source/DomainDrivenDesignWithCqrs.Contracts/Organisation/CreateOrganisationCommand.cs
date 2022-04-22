using MediatR;

namespace DomainDrivenDesignWithCqrs.Contracts.Organisation;

public class CreateOrganisationCommand : CreateOrEditOrganisationModel, IRequest<CreateOrganisationResponse>
{
}
