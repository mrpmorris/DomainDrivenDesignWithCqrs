using System.Text.Json.Serialization;

namespace DomainDrivenDesignWithCqrs.Contracts;

public abstract class PagedItemsResponseBase<T> : ResponseBase
{
	public PagedItemsModel<T> Result { get; private set; } = PagedItemsModel<T>.Empty;
	
	public PagedItemsResponseBase() { }

	[JsonConstructor]
	public PagedItemsResponseBase(PagedItemsModel<T> result)
	{
		Result = result ?? throw new ArgumentNullException(nameof(result));
	}
}
