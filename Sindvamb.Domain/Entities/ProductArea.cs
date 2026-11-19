using Sindvamb.Domain;

namespace Sindvamb.Domain.Entities;

public class ProductArea: EntityBase<Guid>
{   
    public string Code { get; set; }
    public string Description { get; set; }

}
