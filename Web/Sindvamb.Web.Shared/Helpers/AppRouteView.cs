using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Rendering;

using System.Net;
using Sindvamb.Web.Shared.Services.Auth;

namespace Sindvamb.Web.Shared.Helpers
{
    public class AppRouteView : RouteView
    {
        [Inject] public NavigationManager NavigationManager { get; set; }

        [Inject] public AuthenticationStateProvider AuthenticationState { get; set; }

        [Parameter] public Type Layout { get; set; }

        protected override void Render(RenderTreeBuilder builder)
        {
            //var authorize = Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute)) != null;

            var state = (CustomAuthenticationProvider)AuthenticationState;
            var isLoginRoute = RouteData.PageType.UnderlyingSystemType.Name.Equals("Login");

            if (!isLoginRoute && state.User?.Identity?.IsAuthenticated != true)
            {
                var returnUrl = WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery);
                NavigationManager.NavigateTo($"Auth/Login?returnUrl={returnUrl}");
            }
            else
            {
                DefaultLayout = Layout;

                if (isLoginRoute && state.User?.Identity?.IsAuthenticated == true)
                {
                    NavigationManager.NavigateTo($"/");
                }

                base.Render(builder);
            }
        }
    }
}
