using Sindvamb.Domain.DTO;

namespace Sindvamb.Domain.DTO;

public class FileLayoutDTO
{
    public Guid Id { get; set; }

    public string LayoutName { get; set; }

    public short LayoutSize { get; set; }


    public virtual ICollection<FileControlDTO> FileControl { get; set; }
}
