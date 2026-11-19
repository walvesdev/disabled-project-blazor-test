#nullable disable

using Sindvamb.Domain;
#nullable disable

using Sindvamb.Domain.Entities;
using Sindvamb.Domain.Helpers;

namespace Sindvamb.Domain.Entities
{
    public class OrderProperty : EntityBase<Guid>
    {
        public OrderProperty()
        {
            Id = SequentialGuidGenerator.NewSequentialGuid();
        }

        
        public string Key { get; set; }
        public string Value { get; set; }

        public virtual Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
