using Microsoft.AspNetCore.Authorization;

namespace Sindvamb.Web.Shared.Utils.Attributes
{
    public class AuthorizeCustomerAttribute : AuthorizeAttribute
    {
        public AuthorizeCustomerAttribute() : base()
        {
            Roles = $"{AuthRole.CUSTOMER}";
        }
    }
}
