namespace Sindvamb.Domain
{
  
    public class EntityBaseAudit<TKey> : EntityBase<TKey>
    {
        public Guid? CreatorId { get; set; }
        public Guid? ModifierId { get; set; }
        public Guid? DeleterId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
