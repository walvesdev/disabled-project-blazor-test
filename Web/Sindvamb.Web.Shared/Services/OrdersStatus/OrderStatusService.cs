namespace Sindvamb.Web.Shared.Services.OrdersStatus;

public class OrderStatusService : ServiceBase<OrderStatusDTO>, IOrderStatusService
{
    public OrderStatusService(SwalService swalService, ToastService toastService) : base(toastService, swalService) 
    {
        EntitName = nameof(OrderStatus);
    }

}
