using DomainDrivenDesignWithCqrs.AppLayer.Services;
using DomainDrivenDesignWithCqrs.Contracts.Organisation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DomainDrivenDesignWithCqrs.Server.Controllers
{

	[ApiController]
	[Route("api/organisation")]
	public class OrganisationController : ControllerBase
	{
		private readonly IRequestDispatcher Dispatcher;

		public OrganisationController(IRequestDispatcher dispatcher)
		{
			Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
		}

		[Route("create")]
		[HttpPost]
		public Task<CreateOrganisationResponse> CreateAsync(CreateOrganisationCommand command) =>
			Dispatcher.Execute(command);
	}
}
