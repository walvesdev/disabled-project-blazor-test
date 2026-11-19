#nullable disable

using Sindvamb.Domain;
#nullable disable

using Sindvamb.Domain.Entities;

#nullable disable

namespace Sindvamb.Domain.Entities
{
    public class PartnerUnit : EntityBaseAudit<Guid>
    {
        public PartnerUnit()
        {
            Customers = new HashSet<Customer>();
            Users = new HashSet<User>();
        }
        
        public string Name { get; set; }

        public string CpfCnpj { get; set; }

        public string Email { get; set; }

        public bool? Enabled { get; set; }
        
        public virtual Guid PartnerId { get; set; }
        public virtual Partner Partner { get; set; }
        
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Customer> Customers { get; set; }
    }
}
