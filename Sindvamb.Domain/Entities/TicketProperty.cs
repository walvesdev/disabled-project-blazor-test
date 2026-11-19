#nullable disable

using Sindvamb.Domain;
#nullable disable

using Sindvamb.Domain.Entities;

#nullable disable

namespace Sindvamb.Domain.Entities
{
    public class TicketProperty : EntityBase<Guid>
    {
        
        public string Key { get; set; }
        public string Value { get; set; }


        public Guid TicketId { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
