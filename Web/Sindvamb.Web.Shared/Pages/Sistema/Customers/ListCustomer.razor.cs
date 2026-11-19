using Sindvamb.Web.Shared.Models;

namespace Sindvamb.Web.Shared.Pages.Sistema.Customers
{
    public partial class ListCustomer : ListComponentBase<CustomerModel, ICustomerService>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
