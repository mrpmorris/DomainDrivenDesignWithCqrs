using DomainDrivenDesignWithCqrs.AppLayer.Exceptions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DomainDrivenDesignWithCqrs.AppLayer.Persistence;

public class ApplicationDbContext : DbContext
{
  private readonly static Regex UniqueIndexRegex = new Regex(@"'UX_\w+_(\w+)'$");
  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
  {

  }

  public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
  {
		int result = 0;
    try
    {
      result = await base.SaveChangesAsync(cancellationToken);
    }
		catch (DbUpdateConcurrencyException)
		{
			throw new DbConflictException();
		}
		catch (DbUpdateException error)
		when ((error.InnerException is SqlException sqlException) && sqlException.Number == 2601)
		{
			var match = UniqueIndexRegex.Match(sqlException.Message);
			if (!match.Success)
				throw new InvalidOperationException("Unique indexes should be named IX_TableName_ColumnName");

			string columnName = match.Groups[1].Value;
			//validationErrorList.Add(
			//	new ValidationError(memberPath: columnName, errorMessage: "Must be unique"));
		}

		return result;
	}
}
