using DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;
using DomainDrivenDesignWithCqrs.Contracts;
using System.Collections.Immutable;

namespace DomainDrivenDesignWithCqrs.AppLayer.Exceptions;

internal class DomainInvariantViolationException : Exception
{
	public AggregateRoot AggregateRoot { get; }
	public ImmutableArray<ValidationError> Errors { get; }

	public DomainInvariantViolationException(
		AggregateRoot aggregateRoot,
		IEnumerable<ValidationError> validationErrors) : base("One or more domain invariant violations")
	{
		AggregateRoot = aggregateRoot ?? throw new ArgumentNullException(nameof(aggregateRoot));
		ArgumentNullException.ThrowIfNull(aggregateRoot);
		Errors = validationErrors.ToImmutableArray();
	}
}
