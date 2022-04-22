using DomainDrivenDesignWithCqrs.Contracts;
using MediatR;

namespace DomainDrivenDesignWithCqrs.AppLayer.Services;

/// <summary>
/// Dispatches request to handlers via MediatR
/// </summary>
public interface IRequestDispatcher
{
	/// <summary>
	/// Executes a request
	/// </summary>
	/// <typeparam name="TResponse">The response type (inferred from request)</typeparam>
	/// <param name="request">The request</param>
	/// <param name="cancellationToken"></param>
	/// <returns>Response of type TResponse, which must descend from <see cref="ResponseBase"/></returns>
	Task<TResponse> ExecuteAsync<TResponse>(
		IRequest<TResponse> request,
		CancellationToken cancellationToken = default)
		where TResponse : ResponseBase, new();

}
internal class RequestDispatcher : IRequestDispatcher
{
	private readonly IMediator Mediator;

	public RequestDispatcher(IMediator mediator)
	{
		Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
	}

	public Task<TResponse> ExecuteAsync<TResponse>(
		IRequest<TResponse> request,
		CancellationToken cancellationToken = default)
		where TResponse : ResponseBase, new()
	=>
		Mediator.Send(request, cancellationToken);
}
