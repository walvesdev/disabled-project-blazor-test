#nullable disable
using Sindvamb.Domain;
#nullable disable
using Sindvamb.Domain.Entities;

#nullable disable
namespace Sindvamb.Domain.Entities
{

    public class OrderStatus : EntityBase<Guid>
    {
        

        public string Code { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

    }

}
