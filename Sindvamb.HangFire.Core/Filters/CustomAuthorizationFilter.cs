#region

using System.IdentityModel.Tokens.Jwt;
using Hangfire.Annotations;
using Hangfire.Dashboard;
using Sindvamb.Security.Configurations;
using Sindvamb.Utils;

#endregion

namespace Sindvamb.HangFire.Core.Filters
{
    public class CustomAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string loginUrl;
        private readonly string cookieName;

        public CustomAuthorizationFilter(string loginUrl, string cookieName)
        {
            this.loginUrl = loginUrl;
            this.cookieName = cookieName;
        }

        public bool Authorize([NotNull] DashboardContext context)
        {
            //return AppServices.IsDevelopment() ? true : AuthorizePrd(context);
            return  AuthorizePrd(context);
        }

        public bool AuthorizePrd([NotNull] DashboardContext context)
        {
            var httpContext = AppServices.GetHttpContext(); // context.GetHttpContext();

            if (httpContext.Request.Cookies.ContainsKey(cookieName))
            {
                var token = httpContext.Request.Cookies[cookieName];
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token);

                if (jsonToken.Claims.Any(p => p.Type == "role" && (
                        p.Value == AuthPolicy.ADMIN || 
                        p.Value == AuthPolicy.MASTER || 
                        p.Value == AuthPolicy.SUPPORT)) &&
                    jsonToken.ValidTo >= DateTime.UtcNow)
                {
                    return true;
                }
            }

            httpContext.Response.Redirect(loginUrl);
            return true;
        }
    }
}