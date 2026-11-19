namespace Sindvamb.Web.Shared.Services.Orders;

public class OrderService : ServiceBase<OrderDTO>, IOrderService
{
    public OrderService(SwalService swalService, ToastService toastService) : base(toastService, swalService) 
    {
        EntitName = nameof(Order);
    }

    public async Task UpdateStatusIndex(OrderDTO order)
    {
        var http = new HttpClientService<OrderDTO, bool, Guid>($"{EntitName}/UpdateStatusIndex");

        var result = await http.Post(order);

        if (!result.IsSuccessStatus)
        {
            var message = result.TryGetDomainNotification($"Erro ao atualizar situação da demanda. Por favor contate o suporte!");

             await ShowErrorModal(message);
        }

    }
}
