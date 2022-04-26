using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainDrivenDesignWithCqrs.AppLayer.DomainEntities;

internal class EntityBase
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public Guid Id { get; private set; } = Services.SequentialGuidGenerator.Next();
}
