namespace Sindvamb.Web.Shared.Services.OrderTypes;

public class OrderTypeService : ServiceBase<OrderTypeDTO>, IOrderTypeService
{
    public OrderTypeService(SwalService swalService, ToastService toastService) : base(toastService, swalService)
    {
        EntitName = nameof(OrderType);
    }

}
