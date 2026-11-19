using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services.CustomerTypes;

namespace Sindvamb.Web.Shared.Pages.Sistema.CustomerTypes
{
    public partial class AddEditCustomerType : EditComponentBase<CustomerTypeDTO, ICustomerTypeService, int>
    {

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            OperationEntityText = "Tipo de Cliente";
        }
      


    }
}
