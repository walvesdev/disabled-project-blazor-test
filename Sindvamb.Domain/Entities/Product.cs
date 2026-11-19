using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindvamb.Domain.Entities
{
    public class Product : EntityBase<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Tax { get; set; }
        public decimal Price { get; set; }
        public decimal PriceCost { get; set; }
        public int Quantity { get; set; }
        public int MinimumQuantity { get; set; }
        public bool Active { get; set; }


        public virtual Guid PartnerUnitId { get; set; }
        public virtual PartnerUnit PartnerUnit { get; set; }

        public virtual Guid ProductTypetId { get; set; }
        public virtual ProductType ProductType { get; set; }
    }
}
