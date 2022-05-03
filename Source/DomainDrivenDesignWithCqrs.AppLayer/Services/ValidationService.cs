using DomainDrivenDesignWithCqrs.Contracts;
using FluentValidation;
using FluentValidation.Results;
using Morris.DataAnnotations.ObjectTreeValidator;
using System.Collections.Immutable;

namespace DomainDrivenDesignWithCqrs.AppLayer.Services;

public interface IValidationService
{
	Task<IEnumerable<ValidationError>> ValidateAsync(object instance, bool mustHaveAValidator = false);
}

internal class ValidationService : IValidationService
{
	private readonly IValidatorFactory ValidatorFactory;

	public ValidationService(IValidatorFactory validatorFactory)
	{
		ValidatorFactory = validatorFactory ?? throw new ArgumentNullException(nameof(validatorFactory));
	}

	public async Task<IEnumerable<ValidationError>> ValidateAsync(object instance, bool mustHaveAValidator = false)
	{
		ArgumentNullException.ThrowIfNull(instance);

		bool hasValidator = false;
		Type t = instance.GetType();
		var validationErrors = ImmutableArray.CreateBuilder<ValidationError>();
		var validationContext = new ValidationContext<object>(instance);

		while (t != typeof(object))
		{
			IValidator validator = ValidatorFactory.GetValidator(t);
			t = t.BaseType!;

			if (validator is null)
				continue;
			hasValidator = true;

			ValidationResult validationResult = await validator.ValidateAsync(validationContext);
			if (!validationResult.IsValid)
			{
				validationErrors.AddRange(
					validationResult
						.Errors
						.Select(x => new ValidationError(path: x.PropertyName, message: x.ErrorMessage)));
			}
		}
		if (mustHaveAValidator && !hasValidator)
			throw new InvalidOperationException($"Type \"{instance.GetType().Name}\" should have a validator");

		// Add any errors from DataAnnotations
		if (!RecursiveValidator.TryValidateObject(instance, out var recursiveValidationResults))
			validationErrors.AddRange(
					recursiveValidationResults.Select(x => new ValidationError(path: x.FullPath, message: x.ErrorMessage)));

		return validationErrors.ToImmutableList();
	}
}