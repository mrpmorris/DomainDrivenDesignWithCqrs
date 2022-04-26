using DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;
using Microsoft.EntityFrameworkCore;

namespace DomainDrivenDesignWithCqrs.AppLayer.Persistence.Repositories;

internal interface IOrganisationRepository
{
	void AddOrUpdate(Organisation entity);
	Task<Organisation?> GetAsync(Guid id);
	IQueryable<Organisation> Query();
}

internal class OrganisationRepository : RepositoryBase<Organisation>, IOrganisationRepository
{
	public OrganisationRepository(ApplicationDbContext dbContext) : base(dbContext) { }

	protected override DbSet<Organisation> Collection => DbContext.Organisation;
	// Organisation has no aggregate parts
	protected override IQueryable<Organisation> IncludeAggregateParts(IQueryable<Organisation> query) => query;
}
