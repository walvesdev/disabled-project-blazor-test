namespace Sindvamb.Domain.DTO;

public class OrderStatusDTO
{
    public Guid Id { get; set; }
   
    public string Code { get; set; }

   public string Description { get; set; }

    public virtual ICollection<OrderDTO> Orders { get; set; }
}
