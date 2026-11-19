using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services;

namespace Sindvamb.Web.Shared.Services.OrderCategories;

public interface IProductCategoryService : IServiceBase<ProductCategoryDTO>
{
    Task<List<ProductCategoryDTO>> GetAllProductCategory();
}
