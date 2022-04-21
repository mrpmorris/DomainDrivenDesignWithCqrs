using DomainDrivenDesignWithCqrs.AppLayer.Domain;
using DomainDrivenDesignWithCqrs.AppLayer.Exceptions;
using DomainDrivenDesignWithCqrs.AppLayer.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace DomainDrivenDesignWithCqrs.AppLayer.Persistence;

internal class ApplicationDbContext : DbContext
{
	private readonly static Regex UniqueIndexRegex = new Regex(@"'UX_(\w+)_(\w+)'$");
	private readonly IDomainInvariantsGuard DomainInvariantsGuard;

	internal DbSet<Organisation> Organisations { get; set; } = null!;

	public ApplicationDbContext(
		DbContextOptions<ApplicationDbContext> options,
		IDomainInvariantsGuard domainInvariantsGuard) : base(options)
	{
		DomainInvariantsGuard = domainInvariantsGuard ?? throw new ArgumentNullException(nameof(domainInvariantsGuard));

		// UnitOfWork will set this to true, so only updates via
		// UnitOfWork are permitted. Having this disabled improves
		// read-only query performance.
		ChangeTracker.AutoDetectChangesEnabled = false;
	}

	public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
	{
		try
		{
			await CheckDomainInvariantsAsync();
			return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
		}
		catch (DbUpdateConcurrencyException)
		{
			throw new DbConcurrencyException();
		}
		catch (DbUpdateException error)
		when ((error.InnerException is SqlException sqlException) && sqlException.Number == 2601)
		{
			throw CreateDbUniqueIndexViolationException(sqlException);
		}
	}

	internal void EnableChangeTracking()
	{
		ChangeTracker.AutoDetectChangesEnabled = true;
	}

	private Task CheckDomainInvariantsAsync()
	{
		var aggregateRoots = ChangeTracker
			.Entries()
			.Where(x => x.Entity is AggregateRoot)
			.Where(x => x.State != EntityState.Deleted && x.State != EntityState.Unchanged)
			.Select(x => x.Entity)
			.Cast<AggregateRoot>()
			.ToImmutableArray();
		return DomainInvariantsGuard.EnsureAggregateRootsAreValidAsync(aggregateRoots);
	}

	private static DbUniqueIndexViolationException CreateDbUniqueIndexViolationException(SqlException sqlException)
	{
		var match = UniqueIndexRegex.Match(sqlException.Message);
		if (!match.Success)
			throw new InvalidOperationException("Unique indexes should be named IX_TableName_ColumnName");

		string indexName = match.Groups[0].Value;
		string tableName = match.Groups[1].Value;
		string columnName = match.Groups[2].Value;
		return new DbUniqueIndexViolationException(
			indexName: indexName,
			tableName: tableName,
			columnName: columnName);
	}

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
	[Obsolete]
	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		throw new InvalidOperationException();
	}

	[Obsolete]
	public override int SaveChanges()
	{
		throw new InvalidOperationException();
	}
#pragma warning restore CS0809 // Obsolete member overrides non-obsolete member

}
