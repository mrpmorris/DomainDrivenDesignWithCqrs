using DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;
using DomainDrivenDesignWithCqrs.AppLayer.Persistence;
using DomainDrivenDesignWithCqrs.AppLayer.Persistence.Repositories;
using DomainDrivenDesignWithCqrs.AppLayer.Persistence.ViewSources;
using DomainDrivenDesignWithCqrs.AppLayer.RequestHandlers;
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
		RegisterRepositories(services);
		RegisterValidators(services);
		RegisterViewSources(services);
	}

	private static void RegisterCqrsClasses(IServiceCollection services)
	{
		services.AddScoped<IRequestDispatcher, RequestDispatcher>();
		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestValidatorMiddleware<,>));
		services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestErrorHandlerMiddleware<,>));
	}

	private static void RegisterRepositories(IServiceCollection services)
	{
		services.AddScoped<IOrganisationRepository, OrganisationRepository>();
		services.AddScoped<IOrganisationTypeRepository, OrganisationTypeRepository>();
	}

	private static void RegisterValidators(IServiceCollection services)
	{
		services.AddScoped<IValidatorFactory, ServiceProviderValidatorFactory>();
		services.AddScoped<IValidationService, ValidationService>();
		services.AddScoped<IDomainInvariantsGuard, DomainInvariantsGuard>();
		services.AddValidatorsFromAssemblyContaining<Contracts.ValidationError>(includeInternalTypes: true);
		services.AddValidatorsFromAssemblyContaining<EntityBase>(includeInternalTypes: true);
	}

	private static void RegisterViewSources(IServiceCollection services)
	{
		services.AddScoped<IOrganisationSearchItemModelViewSource, OrganisationSearchItemModelViewSource>();
		services.AddScoped<IOrganisationTypeSearchItemModelViewSource, OrganisationTypeSearchItemModelViewSource>();
	}
}
