using DomainDrivenDesignWithCqrs.AppLayer.Exceptions;
using DomainDrivenDesignWithCqrs.AppLayer.Persistence;
using DomainDrivenDesignWithCqrs.Contracts;

namespace DomainDrivenDesignWithCqrs.AppLayer.Services;

public interface IUnitOfWork
{
	Task<IEnumerable<ValidationError>> CommitAsync(CancellationToken cancellationToken = default);

}
internal class UnitOfWork : IUnitOfWork
{
	private readonly ApplicationDbContext DbContext;

	public UnitOfWork(ApplicationDbContext dbContext)
	{
		DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
		dbContext.EnableChangeTracking();
	}

	public async Task<IEnumerable<ValidationError>> CommitAsync(CancellationToken cancellationToken = default)
	{
		var result = new List<ValidationError>();
		try
		{
			await DbContext.SaveChangesAsync(cancellationToken);

			return result;
		}
		catch (DbUniqueIndexViolationException e)
		{
			return new[]
			{
				new ValidationError(path: e.ColumnName, message: "Must be unique")
			};
		}
	}
}
