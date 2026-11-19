using Sindvamb.Domain;
using Sindvamb.Domain.Entities;
using Sindvamb.Domain.Helpers;

namespace Sindvamb.Domain.Entities;

public class OrderEmail : EntityBase<Guid>
{
    public OrderEmail()
    {
        Id = SequentialGuidGenerator.NewSequentialGuid();
    }

    
    public string Email { get; set; }
    
    public virtual Guid OrderId { get; set; }
    public virtual Order Order { get; set; }
}
