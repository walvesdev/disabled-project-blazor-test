using Sindvamb.Domain.DTO;
namespace Sindvamb.Domain.DTO;

public class FileControlDTO
{
    public Guid Id { get; set; }

    public string FileName { get; set; }

    public decimal FileSize { get; set; }

    public byte[] FileArray { get; set; }

    public string? TargetPath { get; set; }

    public string ContentType { get; set; }

    public string? Description { get; set; }

    public bool? Approved { get; set; }

    public Guid? DependentId { get; set; }

    public virtual Guid? UserId { get; set; }

    public Guid? LayoutId { get; set; }

    public virtual FileLayoutDTO Layout { get; set; }
}
