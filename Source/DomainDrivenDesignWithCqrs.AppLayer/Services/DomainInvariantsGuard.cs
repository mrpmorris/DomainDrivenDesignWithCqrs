using DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;
using DomainDrivenDesignWithCqrs.AppLayer.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace DomainDrivenDesignWithCqrs.AppLayer.Services;

internal interface IDomainInvariantsGuard
{
	Task EnsureAggregateRootsAreValidAsync(IEnumerable<AggregateRoot> aggregateRoots);
}

internal class DomainInvariantsGuard : IDomainInvariantsGuard
{
	private readonly IValidationService ValidationService;

	public DomainInvariantsGuard(IValidationService validationService)
	{
		ValidationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
	}

	public async Task EnsureAggregateRootsAreValidAsync(IEnumerable<AggregateRoot> aggregateRoots)
	{
		ArgumentNullException.ThrowIfNull(aggregateRoots);

		foreach(var aggregateRoot in aggregateRoots)
		{
			IEnumerable<Contracts.ValidationError> errors =
				await ValidationService.ValidateAsync(aggregateRoot, mustHaveAValidator: true);
			if (errors.Any())
				throw new DomainInvariantViolationException(aggregateRoot, errors);
		}
	}
}
