using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Data;
using Sindvamb.Web.Shared.Services.CustomerTypes;

namespace Sindvamb.Web.Shared.Pages.Sistema.CustomerTypes
{
    public partial class ListCustomerType : ListComponentBase<CustomerTypeDTO, ICustomerTypeService>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        
    }
}
