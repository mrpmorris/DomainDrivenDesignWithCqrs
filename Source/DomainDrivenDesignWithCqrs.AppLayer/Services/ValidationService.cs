using DomainDrivenDesignWithCqrs.Contracts;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Immutable;

namespace DomainDrivenDesignWithCqrs.AppLayer.Services;

public interface IValidationService
{
	Task<IEnumerable<ValidationError>> ValidateAsync(object instance, bool mustHaveAValidator = false);
}

internal class ValidationService : IValidationService
{
	private readonly ImmutableArray<ValidationError> EmptyErrors = ImmutableArray.Create<ValidationError>();
	private readonly IValidatorFactory ValidatorFactory;

	public ValidationService(IValidatorFactory validatorFactory)
	{
		ValidatorFactory = validatorFactory ?? throw new ArgumentNullException(nameof(validatorFactory));
	}

	public async Task<IEnumerable<ValidationError>> ValidateAsync(object instance, bool mustHaveAValidator = false)
	{
		ArgumentNullException.ThrowIfNull(instance);

		IValidator validator = ValidatorFactory.GetValidator(instance.GetType());
		if (mustHaveAValidator && validator is null)
			throw new InvalidOperationException($"Type \"{instance.GetType().Name}\" should have a validator");

		var context = new ValidationContext<object>(instance);
		ValidationResult validationResult = await validator.ValidateAsync(context);
		if (validationResult.IsValid)
			return EmptyErrors;

		return validationResult
			.Errors
			.Select(x => new ValidationError(path: x.PropertyName, message: x.ErrorMessage))
			.ToImmutableArray();
	}
}