using DomainDrivenDesignWithCqrs.Contracts.Organisations;

namespace DomainDrivenDesignWithCqrs.AppLayer.Persistence.ViewSources;

internal interface IOrganisationSearchItemModelViewSource
{
    IQueryable<OrganisationSearchItemModel> Views { get; }
}

internal class OrganisationSearchItemModelViewSource : IOrganisationSearchItemModelViewSource
{
    private readonly ApplicationDbContext DbContext;

    public OrganisationSearchItemModelViewSource(ApplicationDbContext dbContext)
    {
        DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

	public IQueryable<OrganisationSearchItemModel> Views =>
		from o in DbContext.Organisation
		join t in DbContext.OrganisationType
			on o.TypeId equals t.Id
		select new OrganisationSearchItemModel
		{
			Id = o.Id,
			Name = o.Name,
			Type = t.Name
		};
}
