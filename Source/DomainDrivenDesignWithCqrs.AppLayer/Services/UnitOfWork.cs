using DomainDrivenDesignWithCqrs.AppLayer.Persistence;

namespace DomainDrivenDesignWithCqrs.AppLayer.Services;

public interface IUnitOfWork
{
	Task CommitAsync(CancellationToken cancellationToken = default);

}
internal class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext DbContext;

	public UnitOfWork(ApplicationDbContext dbContext)
	{
		DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		dbContext.EnableChangeTracking();
	}

	public async Task CommitAsync(CancellationToken cancellationToken = default) =>
		await DbContext.SaveChangesAsync(cancellationToken);
}
