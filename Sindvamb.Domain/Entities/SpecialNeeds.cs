namespace Sindvamb.Domain.Entities
{
    public class SpecialNeeds : EntityBase<Guid>
    {
        public string Description { get; set; }

        public virtual Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}
