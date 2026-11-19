#nullable disable

using Sindvamb.Domain;
#nullable disable

using Sindvamb.Domain.Entities;
using Sindvamb.Domain.Helpers;

namespace Sindvamb.Domain.Entities
{
    public class OrderTracking : EntityBase<Guid>
    {
        public OrderTracking()
        {
            Id = SequentialGuidGenerator.NewSequentialGuid();
        }

        
        public DateTime TrackDate { get; set; }
        public string History { get; set; }

        public virtual Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
