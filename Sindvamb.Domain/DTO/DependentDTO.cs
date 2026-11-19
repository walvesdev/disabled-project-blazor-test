namespace Sindvamb.Domain.DTO
{
    public class DependentDTO 
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string? CpfCnpj { get; set; }

        public string? Email { get; set; }

        public string? CellPhone { get; set; }

        public string? Type { get; set; }
        
        public virtual Guid CustomerId { get; set; }
        public virtual Guid UserId { get; set; }

        public List<FileControlDTO> FileControl{ get; set; } = new();


    }
}
