using DomainDrivenDesignWithCqrs.AppLayer.Domain;
using Microsoft.EntityFrameworkCore;

namespace DomainDrivenDesignWithCqrs.AppLayer.Persistence.Repositories;

internal interface IOrganisationTypeRepository
{
	void AddOrUpdate(OrganisationType entity);
	Task<OrganisationType?> GetAsync(Guid id);
	IQueryable<OrganisationType> Query();
}

internal class OrganisationTypeRepository : RepositoryBase<OrganisationType>, IOrganisationTypeRepository
{
	public OrganisationTypeRepository(ApplicationDbContext dbContext) : base(dbContext) { }

	protected override DbSet<OrganisationType> Collection => DbContext.OrganisationType;
	// OrganisationType has no aggregate parts
	protected override IQueryable<OrganisationType> IncludeAggregateParts(IQueryable<OrganisationType> query) => query;
}
