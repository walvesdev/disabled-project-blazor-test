using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services;

namespace Sindvamb.Web.Shared.Services.Orders;

public interface IOrderService : IServiceBase<OrderDTO>
{
    Task UpdateStatusIndex(OrderDTO order);
}
