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
		services.AddMediatR(typeof(Registration).Assembly);
		services.AddAutoMapper(typeof(Registration));
		services.AddScoped<IDateTimeService, DateTimeService>();
		services.AddScoped<IUnitOfWork, UnitOfWork>();
		services.AddDbContext<ApplicationDbContext>(options =>
		{
			options.UseSqlServer(configuration.GetConnectionString("default"));
		});
		RegisterValidators(services);
		RegisterRepositories(services);
	}

	private static void RegisterValidators(IServiceCollection services)
	{
		services.AddScoped<IValidatorFactory, ServiceProviderValidatorFactory>();
		services.AddValidatorsFromAssemblyContaining<Contracts.Validation.Class1>(includeInternalTypes: true);
		services.AddValidatorsFromAssemblyContaining<Domain.EntityBase>(includeInternalTypes: true);
		services.AddScoped<IValidationService, ValidationService>();
		services.AddScoped<IDomainInvariantsGuard, DomainInvariantsGuard>();
	}

	private static void RegisterRepositories(IServiceCollection services)
	{
		services.AddScoped<IOrganisationRepository, OrganisationRepository>();
	}
}
