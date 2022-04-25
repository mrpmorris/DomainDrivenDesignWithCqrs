using System.Text.Json.Serialization;

namespace DomainDrivenDesignWithCqrs.Contracts;

public class PagedItemsModel<T>
{
	public int SearchTotal { get; private set; }
	public int PageNumber { get; private set; }
	public int PageSize { get; private set; }
	public IEnumerable<T> Items { get; private set; }

	public static readonly PagedItemsModel<T> Empty = new(
		searchTotal: 0,
		pageNumber: 1,
		pageSize: 10,
		items: Array.Empty<T>());

	[JsonConstructor]
	public PagedItemsModel(int searchTotal, int pageNumber, int pageSize, IEnumerable<T> items)
	{
		ArgumentNullException.ThrowIfNull(items);
		if (searchTotal < 0)
			throw new ArgumentOutOfRangeException(message: "Cannot be less than 0", paramName: nameof(searchTotal));
		if (pageNumber < 1)
			throw new ArgumentOutOfRangeException(message: "Cannot be less than 1", paramName: nameof(pageNumber));
		if (pageSize < 1)
			throw new ArgumentOutOfRangeException(message: "Cannot be less than 1", paramName: nameof(pageSize));

		SearchTotal = searchTotal;
		PageNumber = pageNumber;
		PageSize = pageSize;
		Items = items.ToArray();
	}

}
