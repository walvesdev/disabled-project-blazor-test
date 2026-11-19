#nullable disable
using Sindvamb.Domain;

#nullable disable
namespace Sindvamb.Domain.Entities
{

    public class ProductCategory : EntityBase<Guid>
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

}
