#nullable disable

using Sindvamb.Domain;
#nullable disable

using Sindvamb.Domain.Entities;

#nullable disable

namespace Sindvamb.Domain.Entities
{
    public class UserRole : EntityBaseNotMaped<int>
    {
        public virtual Guid UserId { get; set; }
        
        public virtual User User { get; set; }

        public virtual Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
