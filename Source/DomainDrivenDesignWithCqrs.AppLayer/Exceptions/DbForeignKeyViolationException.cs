namespace DomainDrivenDesignWithCqrs.AppLayer.Exceptions;

public class DbForeignKeyViolationException : Exception
{
	public string ConstraintName { get; set; }
	public string SourceTableName { get; set; }
	public string TargetTableName { get; set; }
	public string SourceColumnName { get; set; }

	public DbForeignKeyViolationException(
		string constraintName,
		string sourceTableName,
		string targetTableName,
		string sourceColumnName)
		: base("Not found")
	{
		ConstraintName = constraintName ?? throw new ArgumentNullException(nameof(constraintName));
		SourceTableName = sourceTableName ?? throw new ArgumentNullException(nameof(sourceTableName));
		TargetTableName = targetTableName ?? throw new ArgumentNullException(nameof(targetTableName));
		SourceColumnName = sourceColumnName ?? throw new ArgumentNullException(nameof(sourceColumnName));
	}
}
