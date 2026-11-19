using Sindvamb.Domain.DTO;

namespace Sindvamb.Domain.DTO;

public class OrderFileControlDTO
{
    public Guid Id { get; set; }

    public virtual Guid FileControlId { get; set; }
    public virtual FileControlDTO FileControl { get; set; }

    public virtual Guid OrderId { get; set; }
    public virtual OrderDTO Order { get; set; }
}
