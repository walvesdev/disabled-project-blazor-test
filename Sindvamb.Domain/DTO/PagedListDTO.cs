namespace Sindvamb.Domain.DTO;

public class PagedListDTO
{
    public IEnumerable<dynamic> Data { get; set; }

    public long TotalCount { get; set; }
}
