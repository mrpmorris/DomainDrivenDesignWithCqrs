using DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;
using DomainDrivenDesignWithCqrs.AppLayer.Exceptions;
using DomainDrivenDesignWithCqrs.AppLayer.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace DomainDrivenDesignWithCqrs.AppLayer.Persistence;

internal class ApplicationDbContext : DbContext
{
	private readonly static Regex UniqueIndexRegex =
		new Regex(@"'UX_(\w+)_(\w+)'$", RegexOptions.Compiled);
	private readonly static Regex ForeignKeyRegex =
		new Regex(@"""FK_(.*?)_(.*?)_(.*?)""", RegexOptions.Compiled);
	private readonly IDomainInvariantsGuard DomainInvariantsGuard;

	public DbSet<Organisation> Organisation { get; private set; } = null!;
	public DbSet<OrganisationType> OrganisationType { get; private set; } = null!;

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
		catch (DbUpdateException error)
		when ((error.InnerException is SqlException sqlException) && sqlException.Number == 547)
		{
			throw CreateDbForeignKeyViolationException(sqlException);
		}
	}

	internal void EnableChangeTracking()
	{
		ChangeTracker.AutoDetectChangesEnabled = true;
	}

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);

		builder.Entity<Organisation>()
				.HasIndex(x => x.Name)
				.IsUnique();
		builder.Entity<Organisation>()
				.HasOne<OrganisationType>()
				.WithMany()
				.HasForeignKey(x => x.TypeId);
		builder.Entity<OrganisationType>()
				.HasIndex(x => x.Name)
				.IsUnique();
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
			throw new InvalidOperationException("Unique indexes should be named UX_TableName_ColumnName");

		string indexName = match.Groups[0].Value;
		string tableName = match.Groups[1].Value;
		string columnName = match.Groups[2].Value;
		return new DbUniqueIndexViolationException(
				indexName: indexName,
				tableName: tableName,
				columnName: columnName);
	}

	private static DbForeignKeyViolationException CreateDbForeignKeyViolationException(SqlException sqlException)
	{
		var match = ForeignKeyRegex.Match(sqlException.Message);
		if (!match.Success)
			throw new InvalidOperationException("Foreign keys should be named FK_TableName_ForeignTableName_ColumnName");

		string constraintName = match.Groups[0].Value;
		string sourceTableName = match.Groups[1].Value;
		string targetTableName = match.Groups[2].Value;
		string sourceColumnName = match.Groups[3].Value;
		return new DbForeignKeyViolationException(
			constraintName: constraintName,
			sourceTableName: sourceTableName,
			targetTableName: targetTableName,
			sourceColumnName: sourceColumnName);
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
