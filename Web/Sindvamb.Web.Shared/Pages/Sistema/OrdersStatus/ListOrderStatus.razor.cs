using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services.OrdersStatus;

namespace Sindvamb.Web.Shared.Pages.Sistema.OrdersStatus
{
    public partial class ListOrderStatus : ListComponentBase<OrderStatusDTO, IOrderStatusService>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
