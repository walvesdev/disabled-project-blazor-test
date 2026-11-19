using Sindvamb.Domain;
using Sindvamb.Domain.Entities;

namespace Sindvamb.Domain.Entities
{
    public class Audit : EntityBase<Guid>
    {
        public string? TableName { get; set; }
        public string? KeyValues { get; set; }
        public string? OldValues { get; set; }
        public string? NewValues { get; set; }
        public string? IpAddress { get; set; }

        public Guid? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
