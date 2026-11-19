#nullable disable

using Sindvamb.Domain;
#nullable disable

using Sindvamb.Domain.Entities;
using Sindvamb.Domain.Helpers;

namespace Sindvamb.Domain.Entities
{
    public class Ticket : EntityBaseAudit<Guid>
    {
        public Ticket()
        {
            Id = SequentialGuidGenerator.NewSequentialGuid();
            
            TicketProperties = new HashSet<TicketProperty>();
            Attachments = new HashSet<Attachment>();
            OrderEmail = new HashSet<OrderEmail>();
        }

        public string Solution { get; set; }
        
        public DateTime DueDate { get; set; }
       
        public bool? Active { get; set; }
        

        public virtual Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
        
        public virtual Guid CustomerId { get; set; }
        public virtual Customer Customerr { get; set; }

        public virtual ICollection<TicketProperty> TicketProperties { get; set; }
        
        public virtual ICollection<Attachment> Attachments { get; set; }
        
        public virtual ICollection<OrderEmail> OrderEmail { get; set; }
    }
}
