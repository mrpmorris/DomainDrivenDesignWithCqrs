using FluentValidation;

namespace DomainDrivenDesignWithCqrs.Contracts.Validation;

public class Class1
{

}

public class WeatherForecastValidator : AbstractValidator<WeatherForecast>
{
	public WeatherForecastValidator()
	{
		RuleFor(x => x.Summary).NotEmpty();
	}
}
