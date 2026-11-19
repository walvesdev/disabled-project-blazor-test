using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sindvamb.Domain.Entities
{
    public class ProductType : EntityBase<Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual Guid PartnerUnitId { get; set; }
        public virtual PartnerUnit PartnerUnit { get; set; }
    }
}
