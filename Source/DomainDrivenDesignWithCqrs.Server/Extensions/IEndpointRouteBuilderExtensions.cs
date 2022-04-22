using DomainDrivenDesignWithCqrs.AppLayer.Services;
using DomainDrivenDesignWithCqrs.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DomainDrivenDesignWithCqrs.Server.Extensions;

public static class IEndpointRouteBuilderExtensions
{
	public static IEndpointRouteBuilder MapApiGet<TRequest, TResponse>(this IEndpointRouteBuilder endpoints, string url)
		where TRequest : IRequest<TResponse>
		where TResponse : ResponseBase, new()
	{
		endpoints.MapGet(url, ExecuteAsync<TRequest, TResponse>);
		return endpoints;
	}

	public static IEndpointRouteBuilder MapApiPost<TRequest, TResponse>(this IEndpointRouteBuilder endpoints, string url)
		where TRequest : IRequest<TResponse>
		where TResponse : ResponseBase, new()
	{
		endpoints.MapPost(url, ExecuteAsync<TRequest, TResponse>);
		return endpoints;
	}

	private static async Task<TResponse> ExecuteAsync<TRequest, TResponse>([FromBody] TRequest request, [FromServices] IRequestDispatcher dispatcher)
		where TRequest : IRequest<TResponse>
		where TResponse : ResponseBase, new()
	{
		return (await dispatcher.ExecuteAsync(request));
	}
}
