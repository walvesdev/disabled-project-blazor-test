namespace Sindvamb.Domain.Entities
{
    public class Dependent : EntityBase<Guid>
    {

        public Dependent()
        {
            FileControl = new HashSet<FileControl>();
        }

        public string Name { get; set; }

        public string? CpfCnpj { get; set; }

        public string? Email { get; set; }

        public string? CellPhone { get; set; }

        public string? Type { get; set; }
        
        public virtual Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; } = new();

        public virtual ICollection<FileControl> FileControl { get; set; }
    }
}
