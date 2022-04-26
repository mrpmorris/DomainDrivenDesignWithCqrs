using DomainDrivenDesignWithCqrs.AppLayer.Cqrs;
using DomainDrivenDesignWithCqrs.AppLayer.Persistence;
using DomainDrivenDesignWithCqrs.AppLayer.Persistence.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DomainDrivenDesignWithCqrs.AppLayer.Services;

public static class Registration
{
	public static void Register(IServiceCollection services, IConfiguration configuration)
	{
		ArgumentNullException.ThrowIfNull(services);
		ArgumentNullException.ThrowIfNull(configuration);
		services.AddHttpClient();
		services.AddAutoMapper(typeof(Registration).Assembly);
		services.AddMediatR(typeof(Registration).Assembly);
		services.AddScoped<IDateTimeService, DateTimeService>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseSqlServer(configuration.GetConnectionString("default"));
		});
		services.AddSingleton<ISearchService, SearchService>();
		RegisterCqrsClasses(services);
		RegisterValidators(services);
		RegisterRepositories(services);
	}

	private static void RegisterCqrsClasses(IServiceCollection services)
	{
		services.AddScoped<IRequestDispatcher, RequestDispatcher>();
		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidatorMiddleware<,>));
		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestErrorHandlerMiddleware<,>));
	}

	private static void RegisterValidators(IServiceCollection services)
	{
		services.AddScoped<IValidatorFactory, ServiceProviderValidatorFactory>();
		services.AddScoped<IValidationService, ValidationService>();
		services.AddScoped<IDomainInvariantsGuard, DomainInvariantsGuard>();
		services.AddValidatorsFromAssemblyContaining<Contracts.ValidationError>(includeInternalTypes: true);
		services.AddValidatorsFromAssemblyContaining<Domain.EntityBase>(includeInternalTypes: true);
	}

	private static void RegisterRepositories(IServiceCollection services)
	{
		services.AddScoped<IOrganisationRepository, OrganisationRepository>();
		services.AddScoped<IOrganisationTypeRepository, OrganisationTypeRepository>();
	}
}
