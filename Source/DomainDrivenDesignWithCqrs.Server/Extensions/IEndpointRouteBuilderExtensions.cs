using DomainDrivenDesignWithCqrs.AppLayer.Services;
using DomainDrivenDesignWithCqrs.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

namespace DomainDrivenDesignWithCqrs.Server.Extensions;

public static class IEndpointRouteBuilderExtensions
{
	public static IEndpointRouteBuilder MapApiPost<TRequest, TResponse>(this IEndpointRouteBuilder endpoints, string url)
		where TRequest : IRequest<TResponse>
		where TResponse : ResponseBase, new()
	{
		endpoints.MapPost(
			url,
			([FromBody] TRequest request, [FromServices] IRequestDispatcher dispatcher) =>
				ExecuteAsync<TRequest, TResponse>(request, dispatcher));
		return endpoints;
	}

	private static async Task<TResponse> ExecuteAsync<TRequest, TResponse>(TRequest request, IRequestDispatcher dispatcher)
		where TRequest : IRequest<TResponse>
		where TResponse : ResponseBase, new()
	{
		return (await dispatcher.ExecuteAsync(request));
	}
}
