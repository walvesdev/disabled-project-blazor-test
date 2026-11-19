using Sindvamb.Domain.DTO;

namespace Sindvamb.Domain.DTO;

public class OrderEmailDTO
{
    public Guid Id { get; set; }
    public string Email { get; set; }
    
    public virtual Guid OrderId { get; set; }
    public virtual OrderDTO Order { get; set; }
}
