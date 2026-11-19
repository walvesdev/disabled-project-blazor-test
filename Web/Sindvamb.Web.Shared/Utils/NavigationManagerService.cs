using System.Net;

using Microsoft.AspNetCore.Components;

using Sindvamb.Web.Shared.Services.Http;
using Sindvamb.Web.Shared.Utils;

namespace Sindvamb.Web.Shared.Utils
{
    public class NavigationManagerService
    {
        public NavigationManager NavigationManager { get; set; }

        public GenericTokentService TokentService { get; set; }

        public LocalStorage LocalStorageService { get; set; }

        public NavigationManagerService(NavigationManager navigationManager, GenericTokentService tokentService, LocalStorage localStorageService)
        {
            NavigationManager = navigationManager;
            TokentService = tokentService;
            LocalStorageService = localStorageService;
        }

        public async Task NavigateTo(string uri, bool forceLoad = false, bool replace = false)
        {
            if (!uri.Contains("register-user") && !uri.Contains("/Account/Login") && !uri.Contains("recovery-password") && !uri.Contains("update-password"))
            {
                await CheckToken();
            }

            NavigationManager.NavigateTo($"{uri}", forceLoad, replace);
        }

        public async Task CheckToken()
        {
            if (TokentService.Token.IsNullOrWhiteSpace())
            {
                TokentService.Token = AppServices.JwtAccessToken.Token;
            }

            if (TokentService.Expired())
            {
                await LocalStorageService.RemoveAsync("token_was");
            }
        }

    }
}
