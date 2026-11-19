using Sindvamb.Domain;
using Sindvamb.Domain.Entities;

namespace Sindvamb.Domain.Entities
{
    public class PasswordHistory : EntityBaseAudit<Guid>
    {

        public string? OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string SecurityCode { get; set; }
        public bool HasChanged { get; set; }

        public virtual Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
