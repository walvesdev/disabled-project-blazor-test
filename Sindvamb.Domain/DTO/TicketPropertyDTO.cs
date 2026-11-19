using Sindvamb.Domain.DTO;

namespace Sindvamb.Domain.DTO;

public class TicketPropertyDTO
{
    public Guid Id { get; set; }
    public string Key { get; set; }
    public string Value { get; set; }


    public Guid TicketId { get; set; }
    public virtual TicketDTO Ticket { get; set; }
}
