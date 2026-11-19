#nullable disable

using Sindvamb.Domain;
#nullable disable

using Sindvamb.Domain.Entities;
using Sindvamb.Domain.Helpers;

namespace Sindvamb.Domain.Entities
{
    public class OrderFileControl : EntityBase<Guid>
    {
        public OrderFileControl()
        {
            Id = SequentialGuidGenerator.NewSequentialGuid();
        }

        

        public virtual Guid FileControlId { get; set; }
        public virtual FileControl FileControl { get; set; }

        public virtual Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
