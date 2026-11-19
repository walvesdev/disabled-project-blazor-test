#nullable disable

using Sindvamb.Domain;
using Sindvamb.Domain.Entities;

#nullable disable

namespace Sindvamb.Domain.Entities
{
    public class Partner : EntityBaseAudit<Guid>
    {
        public Partner()
        {
            PartnerUnits = new HashSet<PartnerUnit>();
        }

        public string Name { get; set; }

        public string CpfCnpj { get; set; }

        public string Email { get; set; }

        public bool? Enabled { get; set; }
        
        public virtual ICollection<PartnerUnit> PartnerUnits { get; set; }
    }
}
