namespace Sindvamb.Web.Shared.Services.OrderCategories;

public class ProductCategoryService : ServiceBase<ProductCategoryDTO>, IProductCategoryService
{
    public ProductCategoryService(SwalService swalService, ToastService toastService) : base(toastService, swalService)
    {
  
        EntitName = nameof(ProductCategory);
    }

    public async Task<List<ProductCategoryDTO>> GetAllProductCategory()
    {
        var service = new ServiceBase<ProductCategoryDTO>(ToastService, SwalService)
        {
            EntitName = nameof(ProductCategory)
        };

        return await service.GetAll<ProductCategoryDTO>();
    }
}
