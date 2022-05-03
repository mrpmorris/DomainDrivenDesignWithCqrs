using System.ComponentModel.DataAnnotations;

namespace DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;

internal abstract class AggregateRoot : EntityBase
{
	[Timestamp]
	public byte[] Timestamp { get; private set; } = Array.Empty<byte>();
}
