using Sindvamb.Domain.DTO;

namespace Sindvamb.Domain.DTO;

public class TicketDTO
{
    public TicketDTO()
    {
        TicketProperties = new HashSet<TicketPropertyDTO>();
        Attachments = new HashSet<AttachmentDTO>();
        OrderEmail = new HashSet<OrderEmailDTO>();
    }

    public string Solution { get; set; }
        
    public DateTime DueDate { get; set; }
       
    public bool? Active { get; set; }
        

    public virtual Guid OrderId { get; set; }
    public virtual OrderDTO Order { get; set; }
        
    public virtual Guid CustomerId { get; set; }
    public virtual CustomerDTO Customerr { get; set; }

    public virtual ICollection<TicketPropertyDTO> TicketProperties { get; set; }
        
    public virtual ICollection<AttachmentDTO> Attachments { get; set; }
        
    public virtual ICollection<OrderEmailDTO> OrderEmail { get; set; }
}
