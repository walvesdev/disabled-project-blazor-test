using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services.OrderAreas;

namespace Sindvamb.Web.Shared.Pages.Sistema.ProductAreas
{
    public partial class AddEditProductArea : EditComponentBase<ProductAreaDTO, IProductAreaService, int>
    {

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            OperationEntityText = "Área de Demanda";
        }
      


    }
}
