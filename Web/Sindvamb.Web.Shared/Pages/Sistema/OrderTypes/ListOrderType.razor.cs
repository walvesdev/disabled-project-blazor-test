using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services.OrderTypes;

namespace Sindvamb.Web.Shared.Pages.Sistema.OrderTypes
{
    public partial class ListOrderType : ListComponentBase<OrderTypeDTO, IOrderTypeService>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
