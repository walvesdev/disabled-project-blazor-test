

using Sindvamb.Domain;



namespace Sindvamb.Domain.Entities
{
    public class ApplicationConfig : EntityBaseAudit<Guid>
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
