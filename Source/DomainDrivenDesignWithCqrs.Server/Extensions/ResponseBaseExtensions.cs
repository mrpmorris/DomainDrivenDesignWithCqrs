using DomainDrivenDesignWithCqrs.Contracts;
using System.Text.Json;

namespace DomainDrivenDesignWithCqrs.Server.Extensions;

public static class ResponseBaseExtensions
{
	private static readonly JsonSerializerOptions ResponseJsonSerializerOptions = new()
	{
		PropertyNameCaseInsensitive = false,
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = false,
		DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
		Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
	};

	public static IResult AsHttpResult<TResponse>(this TResponse source)
		where TResponse : ResponseBase
	{
		ArgumentNullException.ThrowIfNull(source);
		IResult httpResult =
			source?.Status switch
			{
				null => Results.NoContent(),
				ResponseStatus.BadRequest => Results.BadRequest(source),
				ResponseStatus.ConcurrencyConflict => Results.Conflict(source),
				ResponseStatus.Success => Results.Json(source, ResponseJsonSerializerOptions),
				ResponseStatus.Unauthorized => Results.Unauthorized(),
				ResponseStatus.UnexpectedError => Results.StatusCode(500),
				ResponseStatus.UniqueIndexConflict => Results.Conflict(source),
				_ => throw new NotImplementedException()
			};
		return httpResult;
	}

	public static async ValueTask<IResult> AsHttpResultAsync<TResponse>(this ValueTask<TResponse> source)
		where TResponse : ResponseBase
	=>
		(await source).AsHttpResult();
}
