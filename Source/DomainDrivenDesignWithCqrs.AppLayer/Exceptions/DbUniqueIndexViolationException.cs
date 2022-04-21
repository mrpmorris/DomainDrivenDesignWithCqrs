namespace DomainDrivenDesignWithCqrs.AppLayer.Exceptions;

internal class DbUniqueIndexViolationException : Exception
{
	public string IndexName { get; set; }
	public string TableName { get; set; }
	public string ColumnName { get; set; }

	public DbUniqueIndexViolationException(string indexName, string tableName, string columnName)
		: base($"Unique index violation")
	{
		if (string.IsNullOrEmpty(indexName))
			throw new ArgumentNullException(nameof(indexName));
		if (string.IsNullOrWhiteSpace(tableName))
			throw new ArgumentNullException(nameof(tableName));
		if (string.IsNullOrWhiteSpace(columnName))
			throw new ArgumentNullException(nameof(columnName));

		IndexName = indexName;
		TableName = tableName;
		ColumnName = columnName;
	}
}
