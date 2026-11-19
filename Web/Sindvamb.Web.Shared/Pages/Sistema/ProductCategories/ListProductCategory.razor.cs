using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services.OrderCategories;

namespace Sindvamb.Web.Shared.Pages.Sistema.ProductCategories
{
    public partial class ListProductCategory : ListComponentBase<ProductCategoryDTO, IProductCategoryService>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
