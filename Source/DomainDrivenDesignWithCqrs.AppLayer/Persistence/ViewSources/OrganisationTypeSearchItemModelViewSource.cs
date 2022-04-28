using DomainDrivenDesignWithCqrs.Contracts.OrganisationTypes;

namespace DomainDrivenDesignWithCqrs.AppLayer.Persistence.ViewSources;

internal interface IOrganisationTypeSearchItemModelViewSource
{
	IQueryable<OrganisationTypeSearchItemModel> Views { get; }
}

internal class OrganisationTypeSearchItemModelViewSource : IOrganisationTypeSearchItemModelViewSource
{
	private ApplicationDbContext DbContext;

	public OrganisationTypeSearchItemModelViewSource(ApplicationDbContext dbContext)
	{
		DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
	}

	public IQueryable<OrganisationTypeSearchItemModel> Views =>
		from t in DbContext.OrganisationType
		select
			new OrganisationTypeSearchItemModel
			{
				Id = t.Id,
				Name = t.Name
			};
}
