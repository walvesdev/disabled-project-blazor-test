using Microsoft.AspNetCore.Components;

namespace Sindvamb.Web.Shared.Pages.Sistema.Orders
{
    public partial class ListOrder : ListComponentBase<OrderDTO, IOrderService>
    {
        [Inject] public IProductCategoryService ProductCategoryService { get; set; }
        
        public List<ProductCategoryDTO> ProductCategoryList { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            base.OnInitializedAsync();
            await GetAllProductCategory();
        }

        public async Task GetAllProductCategory()
        {
            ProductCategoryList = await ProductCategoryService.GetAllProductCategory();
        }
    }
}
