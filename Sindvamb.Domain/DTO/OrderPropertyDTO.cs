using Sindvamb.Domain.DTO;

namespace Sindvamb.Domain.DTO;

public class OrderPropertyDTO
{
    public  Guid Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }

    public virtual Guid OrderId { get; set; }
    public virtual OrderDTO Order { get; set; }
}
