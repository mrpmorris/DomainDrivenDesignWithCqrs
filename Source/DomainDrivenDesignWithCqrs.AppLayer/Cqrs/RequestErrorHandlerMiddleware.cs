using DomainDrivenDesignWithCqrs.AppLayer.Exceptions;
using DomainDrivenDesignWithCqrs.Contracts;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DomainDrivenDesignWithCqrs.AppLayer.Cqrs;

internal class RequestErrorHandlerMiddleware<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
	where TRequest : IRequest<TResponse>
	where TResponse : ResponseBase, new()
{
	private readonly ILogger<RequestErrorHandlerMiddleware<TRequest, TResponse>> Logger;

	public RequestErrorHandlerMiddleware(ILogger<RequestErrorHandlerMiddleware<TRequest, TResponse>> logger)
	{
		Logger = logger ?? throw new ArgumentNullException(nameof(logger));
	}

	public async Task<TResponse> Handle(
		TRequest request,
		CancellationToken cancellationToken,
		RequestHandlerDelegate<TResponse> next)
	{
		try
		{
			return await next();
		}
		catch (DbConcurrencyException)
		{
			return new TResponse { Status = ResponseStatus.ConcurrencyConflict };
		}
		catch (DbUniqueIndexViolationException ex)
		{
			var errors = new[] { new ValidationError(path: ex.ColumnName, message: ex.Message) };
			return new TResponse { Status = ResponseStatus.BadRequest, ValidationErrors = errors };
		}
		catch (DomainInvariantViolationException ex)
		{
			Logger.LogError(
				exception: ex,
				message:
					"One or more domain invariants were violated on class {DomainClass}"
					+ " when dispatching {RequestType}. {ValidationErrors}",
				ex.AggregateRoot.GetType().Name, typeof(TRequest).Name, ex.Errors);
			return new TResponse { Status = ResponseStatus.UnexpectedError };
		}
		catch (Exception ex)
		{
			Logger.LogError(ex, "Unexpected error dispatching {RequestType}", typeof(TRequest).Name);
			return new TResponse { Status = ResponseStatus.UnexpectedError };
		}
	}
}
