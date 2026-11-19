using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services.OrderCategories;

namespace Sindvamb.Web.Shared.Pages.Sistema.ProductCategories
{
    public partial class AddEditProductCategory : EditComponentBase<ProductCategoryDTO, IProductCategoryService, int>
    {

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            OperationEntityText = "Area da Demanda";
        }
      


    }
}
