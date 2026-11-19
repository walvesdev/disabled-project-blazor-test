using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sindvamb.Domain.DTO;

namespace Sindvamb.Domain.DTO
{
    public class PartnerUnitDTO
    {
        public PartnerUnitDTO()
        {
            Orders = new HashSet<OrderDTO>();
            Customers = new HashSet<CustomerDTO>();
        }

        public virtual Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? CpfCnpj { get; set; }

        public string? Email { get; set; }
        public bool Enabled { get; set; }

        public virtual Guid? PartnerId { get; set; }

        public virtual PartnerDTO? Partner { get; set; } = new();

        public virtual ICollection<OrderDTO>? Orders { get; set; }
        public virtual ICollection<CustomerDTO>? Customers { get; set; }
    }
}
