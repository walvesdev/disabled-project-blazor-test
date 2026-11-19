using System.ComponentModel.DataAnnotations;

namespace Sindvamb.Domain.DTO;

public class CustomerTypeDTO
{
    public Guid Id { get; set; }
    
    public string Code { get; set; }

    public string Description { get; set; }
}
