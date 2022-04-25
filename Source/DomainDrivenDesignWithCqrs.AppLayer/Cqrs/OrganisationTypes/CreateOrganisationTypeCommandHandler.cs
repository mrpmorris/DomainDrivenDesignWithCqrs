using AutoMapper;
using DomainDrivenDesignWithCqrs.AppLayer.Domain;
using DomainDrivenDesignWithCqrs.AppLayer.Persistence.Repositories;
using DomainDrivenDesignWithCqrs.AppLayer.Services;
using DomainDrivenDesignWithCqrs.Contracts.OrganisationTypes;
using MediatR;

namespace DomainDrivenDesignWithCqrs.AppLayer.Cqrs.OrganisationTypes;

internal class CreateOrganisationTypeCommandHandler : IRequestHandler<CreateOrganisationTypeCommand, CreateOrganisationTypeResponse>
{
	private readonly IOrganisationTypeRepository Repository;
	private readonly IMapper Mapper;
	private readonly IUnitOfWork UnitOfWork;

	public CreateOrganisationTypeCommandHandler(
		IOrganisationTypeRepository repository,
		IMapper mapper,
		IUnitOfWork unitOfWork)
	{
		Repository = repository ?? throw new ArgumentNullException(nameof(repository));
		Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
		UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<CreateOrganisationTypeResponse> Handle(
		CreateOrganisationTypeCommand request,
		CancellationToken cancellationToken)
	{
		var newOrganisationType = Mapper.Map<OrganisationType>(request);
		Repository.AddOrUpdate(newOrganisationType);
		await UnitOfWork.CommitAsync(cancellationToken);
		return new CreateOrganisationTypeResponse(newOrganisationType.Id);
	}
}
