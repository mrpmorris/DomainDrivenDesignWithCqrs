using AutoMapper;
using DomainDrivenDesignWithCqrs.AppLayer.Domain;
using DomainDrivenDesignWithCqrs.AppLayer.Persistence.Repositories;
using DomainDrivenDesignWithCqrs.AppLayer.Services;
using DomainDrivenDesignWithCqrs.Contracts.Organisations;
using MediatR;

namespace DomainDrivenDesignWithCqrs.AppLayer.Cqrs.Organisations;

internal class CreateOrganisationCommandHandler : IRequestHandler<CreateOrganisationCommand, CreateOrganisationResponse>
{
	private readonly IOrganisationRepository Repository;
	private readonly IMapper Mapper;
	private readonly IUnitOfWork UnitOfWork;

	public CreateOrganisationCommandHandler(
		IOrganisationRepository repository,
		IMapper mapper,
		IUnitOfWork unitOfWork)
	{
		Repository = repository ?? throw new ArgumentNullException(nameof(repository));
		Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<CreateOrganisationResponse> Handle(
		CreateOrganisationCommand request,
		CancellationToken cancellationToken)
	{
		var newOrganisation = Mapper.Map<Organisation>(request);
		Repository.AddOrUpdate(newOrganisation);
		await UnitOfWork.CommitAsync(cancellationToken);
		return new CreateOrganisationResponse(newOrganisation.Id);
	}
}
