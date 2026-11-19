using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services.OrderTypes;

namespace Sindvamb.Web.Shared.Pages.Sistema.OrderTypes
{
    public partial class AddEditOrderType : EditComponentBase<OrderTypeDTO, IOrderTypeService, int>
    {

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            OperationEntityText = "Tipo de Demanda";
        }
      


    }
}
