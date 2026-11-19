namespace Sindvamb.Domain.DTO;

public class AttachmentDTO
{
    public Guid Id { get; set; }
        
    public AttachmentDTO()
    {
    }

    public decimal Size { get; set; }
    public string Name { get; set; }
    public string ContentType { get; set; }
    public bool IsPublic { get; set; }
    public string Description { get; set; }
    public string Path { get; set; }
    public string AbsoluteUrl { get; set; }
    public bool InCloud { get; set; }
    public byte[] FileData { get; set; }
}
