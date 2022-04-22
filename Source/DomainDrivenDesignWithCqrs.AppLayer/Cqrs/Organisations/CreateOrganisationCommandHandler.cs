using DomainDrivenDesignWithCqrs.Contracts.Organisation;
using MediatR;

namespace DomainDrivenDesignWithCqrs.AppLayer.Cqrs.Organisations;

public class CreateOrganisationCommandHandler : IRequestHandler<CreateOrganisationCommand, CreateOrganisationResponse>
{
	public Task<CreateOrganisationResponse> Handle(CreateOrganisationCommand request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
