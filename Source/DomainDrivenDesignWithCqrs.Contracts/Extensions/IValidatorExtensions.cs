using FluentValidation;
using FluentValidation.Results;
using System.Collections.Immutable;

namespace DomainDrivenDesignWithCqrs.Contracts.Extensions;

internal static class IValidatorExtensions
{
	public static IValidator<T> Combine<T>(this IEnumerable<IValidator<T>> validators)
	{
		ArgumentNullException.ThrowIfNull(validators);
		if (validators.Count() == 1)
			return validators.First();

		return new CombinedValidator<T>(validators);
	}

	private class CombinedValidator<T> : IValidator<T>
	{
		private readonly IEnumerable<IValidator<T>> Validators;

		public CombinedValidator(IEnumerable<IValidator<T>> validators)
		{
			Validators = validators;
		}

		public bool CanValidateInstancesOfType(Type type) =>
			Validators.Any(x => x.CanValidateInstancesOfType(type));

		public IValidatorDescriptor CreateDescriptor() => Validators.First().CreateDescriptor();

		public ValidationResult Validate(T instance) =>
			Validate(new ValidationContext<T>(instance));

		public ValidationResult Validate(IValidationContext context)
		{
			var failures = ImmutableArray.CreateBuilder<ValidationFailure>();
			foreach (IValidator<T> validator in Validators)
			{
				ValidationResult individualResult = validator.Validate(context);
				if (!individualResult.IsValid)
					failures.AddRange(individualResult.Errors);
			}
			return new ValidationResult(failures.ToImmutable());
		}

		public Task<ValidationResult?> ValidateAsync(T instance, CancellationToken cancellation = default) =>
			ValidateAsync(new ValidationContext<T>(instance), cancellation);

		public async Task<ValidationResult?> ValidateAsync(
			IValidationContext context,
			CancellationToken cancellation = default)
		{
			var failures = ImmutableArray.CreateBuilder<ValidationFailure>();
			foreach (IValidator<T> validator in Validators)
			{
				ValidationResult individualResult = await validator.ValidateAsync(context, cancellation);
				if (cancellation.IsCancellationRequested)
					return null;

				if (!individualResult.IsValid)
					failures.AddRange(individualResult.Errors);
			}
			return new ValidationResult(failures.ToImmutable());
		}
	}
}
