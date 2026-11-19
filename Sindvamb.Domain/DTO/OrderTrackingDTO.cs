using Sindvamb.Domain.DTO;

namespace Sindvamb.Domain.DTO;

public class OrderTrackingDTO
{
    public Guid Id { get; set; }
    public DateTime TrackDate { get; set; }
    public string History { get; set; }

    public virtual Guid OrderId { get; set; }
    public virtual OrderDTO Order { get; set; }
}
