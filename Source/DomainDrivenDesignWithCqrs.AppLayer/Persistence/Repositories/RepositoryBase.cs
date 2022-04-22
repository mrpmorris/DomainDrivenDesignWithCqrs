using DomainDrivenDesignWithCqrs.AppLayer.Domain;
using DomainDrivenDesignWithCqrs.AppLayer.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.Immutable;

namespace DomainDrivenDesignWithCqrs.AppLayer.Persistence.Repositories;

/// <summary>
///		A base class from which other repositories can descend to gain some
///		default behaviours such as caching.
/// </summary>
/// <typeparam name="T">The aggregate root class</typeparam>
internal abstract class RepositoryBase<T>
	where T : AggregateRoot
{
	protected readonly ApplicationDbContext DbContext;
	/// <summary>
	///		Descendant classes should return the correct DbSet from <see cref="DbContext"/>
	/// </summary>
	protected abstract DbSet<T> Collection { get; }
	/// <summary>
	///		When an aggregate root consists of multiple objects
	///		(e.g. PurchaseOrder 1..* PurcharOrderLine) then these
	///		should be included when fetched for modification, so the
	///		aggregate is updated in its entirety, and to avoid lazy loading.
	/// </summary>
	/// <param name="query"></param>
	/// <returns></returns>
	protected abstract IQueryable<T> IncludeAggregateParts(IQueryable<T> query);

	private readonly Dictionary<Guid, T> Cache = new();

	public RepositoryBase(ApplicationDbContext applicationDbContext)
	{
		DbContext = applicationDbContext ?? throw new ArgumentNullException(nameof(applicationDbContext));
	}

	/// <summary>
	///		A queryable collection for the entity
	/// </summary>
	public virtual IQueryable<T> Query() => Collection.AsQueryable();

	/// <summary>
	///		Ensures an entity is saved when the <see cref="Services.UnitOfWork"/> is committed
	/// </summary>
	/// <param name="entity"></param>
	public virtual void AddOrUpdate(T entity)
	{
		ArgumentNullException.ThrowIfNull(entity);

		EntityEntry<T> entityEntry = DbContext.Entry(entity);
		if (entityEntry is null || entityEntry.State == EntityState.Detached)
		{
			Collection.Add(entity);
			Cache.Add(entity.Id, entity);
		}
		else if (entityEntry.State == EntityState.Unchanged)
			entityEntry.State = EntityState.Modified;
	}

	/// <summary>
	///		Ensures entities are saved when the <see cref="Services.UnitOfWork"/> is committed
	/// </summary>
	/// <param name="entities"></param>
	public virtual void AddOrUpdate(IEnumerable<T> entities)
	{
		ArgumentNullException.ThrowIfNull(entities);

		foreach (T entity in entities)
			AddOrUpdate(entity);
	}

	/// <summary>
	///		Returns a cached entity if already loaded, otherwise
	///		retrieves the entity from persistence and caches it.
	///		<para>
	///			<see cref="AggregateLoaded(T)"/> will be called for each
	///			newly loaded entity.
	///		</para>
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public async Task<T?> GetAsync(Guid id)
	{
		if (Cache.TryGetValue(id, out T? result))
			return result;

		result = await IncludeAggregateParts(Query())
			.Where(x => x.Id == id)
			.FirstOrDefaultAsync();

		if (result is not null)
			AggregateLoaded(result);

		return result;
	}

	/// <summary>
	/// Returns all entities currently cached
	/// </summary>
	public IEnumerable<T> GetCachedEntities() => Cache.Values;

	/// <summary>
	///		Gets many cached entities from the persistence and caches
	///		them. Entities will be return from the cache if already loaded.
	///		<para>
	///			No error will be reasied if any of the objects are not found.
	///		</para>
	///		<para>
	///			<see cref="AggregateLoaded(T)"/> will be called for each
	///			newly loaded entity.
	///		</para>
	/// </summary>
	/// 	/// <param name="id"></param>
	/// <returns>Entities that were either present in the Cache or persistence</returns>
	public virtual async Task<IEnumerable<T>> GetManyAsync(IEnumerable<Guid> ids)
	{
		if (ids is null)
			throw new ArgumentNullException(nameof(ids));

		ids = ids.Distinct().ToArray();

		IEnumerable<T> cachedEntities = GetCachedEntities().Where(x => ids.Contains(x.Id));
		IEnumerable<Guid> missingEntityIds = ids.Except(cachedEntities.Select(x => x.Id));

		// Fetch all entities we need that are not already in memory
		T[] fetchedEntities = await
			(
				from aggregateRoot in IncludeAggregateParts(Collection)
				where missingEntityIds.Contains(aggregateRoot.Id)
				select aggregateRoot
			)
			.ToArrayAsync();

		// Cache fetched entities
		foreach (T TEntity in fetchedEntities)
			AggregateLoaded(TEntity);

		return cachedEntities.Union(fetchedEntities);
	}

	/// <summary>
	///		Fetches an entity from the DB and ensures its
	///		<see cref="AggregateRoot.RowVersion"/> has not
	///		been changed since it was last retrieved.
	/// </summary>
	/// <para>
	///		This allows an API to send a DTO to a client along
	///		with a <see cref="AggregateRoot.RowVersion"/>, so that
	///		when the client instructs the API to save changes the
	///		server can ensure no other process has updated the
	///		entity in the meantime.
	/// </para>
	/// <param name="id">The <see cref="EntityBase.Id"/> of the entity.</param>
	/// <param name="rowVersion">The version that was previously retrieved</param>
	/// <returns>The requested entity or null</returns>
	/// <exception cref="DbConcurrencyException"></exception>
	public async Task<T?> GetWithRowVersionAsync(
		Guid id,
		byte[] rowVersion)
	{
		ArgumentNullException.ThrowIfNull(rowVersion);

		T? result = await GetAsync(id);
		if (result is not null)
		{
			if (!Enumerable.SequenceEqual(result.RowVersion, rowVersion))
				throw new DbConcurrencyException();
		}
		return result;
	}

	/// <summary>
	/// Removes an entity from the repository cache and
	/// the Entity Framework cache
	/// </summary>
	/// <param name="entity">The entity to remove</param>
	public virtual void Unload(T entity)
	{
		ArgumentNullException.ThrowIfNull(entity);

		var entityEntry = DbContext.Entry(entity);
		if (entityEntry is not null && entityEntry.State != EntityState.Detached)
		{
			entityEntry.State = EntityState.Detached;
			if (Cache.ContainsKey(entity.Id))
				AggregateUnloaded(entity);
		}
	}

	/// <summary>
	///		Removes multiple entities from the repository cache and
	///		the Entity Framework cache
	/// </summary>
	/// <param name="entities">The entities to remove</param>
	public virtual void Unload(IEnumerable<T> entities)
	{
		ArgumentNullException.ThrowIfNull(entities);

		foreach (T entity in entities)
			Unload(entity);
	}

	/// <summary>
	///		Called each time an <see cref="AggregateRoot"/> is loaded via one
	///		of the other repository methods. This enables descendant repository classes
	///		to keep secondary caches based on different entity properties, e.g.
	///		a cache by Organisation.Code
	///		<para>
	///			Note that the keys should be immutable properties, otherwise
	///			the cache lookup will become invalid.
	///		</para>
	/// </summary>
	/// <param name="entity">The entity that was loaded</param>
	protected virtual void AggregateLoaded(T entity)
	{
		ArgumentNullException.ThrowIfNull(entity);
		Cache[entity.Id] = entity;
	}

	/// <summary>
	///		Called for each unloaded entity whenever <see cref="Unload(T)"/>
	///		or <see cref="Unload(IEnumerable{T})"/> is called. This is so
	///		any descendant repository classes can remove the entity from
	///		any secondary caches. 
	/// </summary>
	/// <seealso cref="AggregateLoaded(T)"/>
	/// <param name="entity">The entity that was unloaded</param>
	protected virtual void AggregateUnloaded(T entity)
	{
		ArgumentNullException.ThrowIfNull(entity);
		Cache.Remove(entity.Id);
	}
}
