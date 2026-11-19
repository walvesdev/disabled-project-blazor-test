#nullable disable

using Sindvamb.Domain;
#nullable disable

using Sindvamb.Domain.Entities;

#nullable disable

namespace Sindvamb.Domain.Entities
{
    public class TicketStatus : EntityBase<Guid>
    {
        
        
        public TicketStatus()
        {
            Tickets = new HashSet<Ticket>();
        }

        public string Code { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
