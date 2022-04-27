using DomainDrivenDesignWithCqrs.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainDrivenDesignWithCqrs.AppLayer.Services;

public interface ISearchService
{
	Task<PagedItemsModel<T>> SearchAsync<T>(
		IQueryable<T> source,
		int pageNumber,
		int itemsPerPage = 10,
		int? maxAllowedItemsPerPage = null);
}

internal class SearchService : ISearchService
{
	public async Task<PagedItemsModel<T>> SearchAsync<T>(
		IQueryable<T> source,
		int pageNumber,
		int itemsPerPage,
		int? maxAllowedItemsPerPage)
	{
		if (pageNumber < 1)
			throw new ArgumentOutOfRangeException(message: "Must be at least 1", paramName: nameof(pageNumber));
		if (itemsPerPage < 1)
			throw new ArgumentOutOfRangeException(message: "Must be at least 1", paramName: nameof(itemsPerPage));
		ArgumentNullException.ThrowIfNull(source);

		if (maxAllowedItemsPerPage > 0)
			itemsPerPage = Math.Min(itemsPerPage, maxAllowedItemsPerPage.Value);

		T[] items = Array.Empty<T>();
		
		int count = await source.CountAsync();
		int maxPossiblePageNumber = Math.Max(1, (int)Math.Ceiling(count / (decimal)itemsPerPage));
		pageNumber = Math.Min(pageNumber, maxPossiblePageNumber);
		if (count > 0)
		{
			int skipAmount = (pageNumber - 1) * itemsPerPage;
			IQueryable<T> paged = source.Skip(skipAmount).Take(itemsPerPage);
			items = await paged.ToArrayAsync();
		}
		return new PagedItemsModel<T>(
			searchTotal: count,
			pageNumber: pageNumber,
			pageSize: itemsPerPage,
			items: items);
	}
}
