using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DomainDrivenDesignWithCqrs.AppLayer.Domain;

internal class EntityBase
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public Guid Guid { get; private set; } = Services.SequentialGuidGenerator.Next();
}
