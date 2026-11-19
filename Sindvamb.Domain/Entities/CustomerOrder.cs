using Sindvamb.Domain;
using Sindvamb.Domain.Entities;
using Sindvamb.Domain.Helpers;

namespace Sindvamb.Domain.Entities;

public class CustomerOrder : EntityBaseAudit<Guid>
{
    public CustomerOrder()
    {
        Id = SequentialGuidGenerator.NewSequentialGuid();
    }

    public bool IsSindvamb { get; set; }
    public bool IsPresidency { get; set; }
    public bool IsClient { get; set; }
    
    public bool IsDirector { get; set; }
    
    public bool IsManager { get; set; }

    public virtual Guid OrderId { get; set; }
    public virtual Order Order { get; set; }

    public virtual Guid CustomerId { get; set; }
    public virtual Customer Customer { get; set; }
}
