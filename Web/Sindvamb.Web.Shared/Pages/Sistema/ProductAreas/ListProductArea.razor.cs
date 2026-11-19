using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services.OrderAreas;

namespace Sindvamb.Web.Shared.Pages.Sistema.ProductAreas
{
    public partial class ListProductArea : ListComponentBase<ProductAreaDTO, IProductAreaService>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
