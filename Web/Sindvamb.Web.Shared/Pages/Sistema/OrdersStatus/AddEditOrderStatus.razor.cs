using Sindvamb.Domain.DTO;

using Sindvamb.Web.Shared.Services.OrdersStatus;

namespace Sindvamb.Web.Shared.Pages.Sistema.OrdersStatus
{
    public partial class AddEditOrderStatus : EditComponentBase<OrderStatusDTO, IOrderStatusService, int>
    {

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            OperationEntityText = "Situação da Demanda";
        }
      


    }
}
