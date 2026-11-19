using Microsoft.AspNetCore.Authorization;

namespace Sindvamb.Web.Shared.Utils.Attributes
{
    public class AuthorizeAdminAttribute : AuthorizeAttribute
    {
        public AuthorizeAdminAttribute() : base()
        {
            Roles = $"{AuthRole.MASTER},{AuthRole.ADMIN}";
        }
    }
}
