using Microsoft.AspNetCore.Authorization;

namespace Sindvamb.Web.Shared.Utils.Attributes
{
    public class AuthorizeMasterAttribute : AuthorizeAttribute
    {
        public AuthorizeMasterAttribute() : base()
        {
            Roles = $"{AuthRole.MASTER}";
        }
    }
}
