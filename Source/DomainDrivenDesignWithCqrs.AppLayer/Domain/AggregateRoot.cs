using System.ComponentModel.DataAnnotations;

namespace DomainDrivenDesignWithCqrs.AppLayer.Domain;

internal abstract class AggregateRoot : EntityBase
{
	[Timestamp]
	public byte[]? RowVersion { get; private set; }
}
