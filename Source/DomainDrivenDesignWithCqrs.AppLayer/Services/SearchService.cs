using DomainDrivenDesignWithCqrs.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainDrivenDesignWithCqrs.AppLayer.Services;

public interface ISearchService
{
	Task<PagedItemsModel<T>> SearchAsync<T>(
		int pageNumber,
		int pageSize,
		IQueryable<T> source,
		Func<IQueryable<T>, IQueryable<T>> addFilter,
		Func<IQueryable<T>, IQueryable<T>> addOrdering);
}

internal class SearchService : ISearchService
{
	public async Task<PagedItemsModel<T>> SearchAsync<T>(
		int pageNumber,
		int pageSize,
		IQueryable<T> source,
		Func<IQueryable<T>, IQueryable<T>> addFilter,
		Func<IQueryable<T>, IQueryable<T>> addOrdering)
	{
		if (pageNumber < 1)
			throw new ArgumentOutOfRangeException(message: "Must be at least 1", paramName: nameof(pageNumber));
		if (pageSize < 1)
			throw new ArgumentOutOfRangeException(message: "Must be at least 1", paramName: nameof(pageSize));
		ArgumentNullException.ThrowIfNull(source);
		ArgumentNullException.ThrowIfNull(addFilter);

		IQueryable<T> filtered = addFilter(source);
		T[] items = Array.Empty<T>();

		int count = await filtered.CountAsync();
		int maxPossiblePageNumber = Math.Max(1, (int)Math.Ceiling(count / (decimal)pageSize));
		pageNumber = Math.Min(pageNumber, maxPossiblePageNumber);
		if (count > 0)
		{
			int skipAmount = (pageNumber - 1) * pageSize;
			IQueryable<T> ordered = addOrdering?.Invoke(filtered) ?? filtered;
			IQueryable<T> paged = ordered.Skip(skipAmount).Take(pageSize);
			items = await paged.ToArrayAsync();
		}
		return new PagedItemsModel<T>(
			searchTotal: count,
			pageNumber: pageNumber,
			pageSize: pageSize,
			items: items);
	}
}
