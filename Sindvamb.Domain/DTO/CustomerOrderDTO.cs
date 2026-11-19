namespace Sindvamb.Domain.DTO;

public class CustomerOrderDTO
{
    public Guid Id { get; set; }
    public bool IsSindvamb { get; set; }
    
    public bool IsPresidency { get; set; }
    
    public bool IsClient { get; set; }
    
    public bool IsDirector { get; set; }
    
    public bool IsManager { get; set; }

    public virtual Guid OrderId { get; set; }
    public virtual OrderDTO Order { get; set; }

    public virtual Guid CustomerId { get; set; }
    public virtual CustomerDTO Customer { get; set; }
}
