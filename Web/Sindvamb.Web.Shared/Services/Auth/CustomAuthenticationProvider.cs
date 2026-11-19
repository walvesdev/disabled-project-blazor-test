using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using Microsoft.AspNetCore.Components.Authorization;
using Sindvamb.Web.Shared.Services.Auth;
using Sindvamb.Web.Shared.Services.Http;


namespace Sindvamb.Web.Shared.Services.Auth
{
    public class CustomAuthenticationProvider : AuthenticationStateProvider
    {
        private LocalStorage _localStorageService;
        public ClaimsPrincipal User { get; set; }
        public GenericTokentService TokentService { get; set; }
        public NavigationManagerService NavigationManager { get; set; }




        public CustomAuthenticationProvider(
            LocalStorage localStorageService,
            StateContainer stateContainer,
            GenericTokentService tokentService,
            NavigationManagerService navigationManager)
        {
            _localStorageService = localStorageService;
            TokentService = tokentService;
            NavigationManager = navigationManager;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _localStorageService.GetAsync<JwtAccessToken>("token_was");

                if (token.IsNull() || token.Token.IsNullOrWhiteSpace())
                {
                    var anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                    //await NavigationManager.NavigateTo("/Account/Login", true, true);
                    return anonymous;
                }

                TokentService.Token = token.Token;
                TokentService.Username = token.Name;
                TokentService.ExpirationDate = Convert.ToDateTime(token.Expiration);

                await NavigationManager.CheckToken();

                var identity = new ClaimsIdentity(
                    new GenericIdentity(token.Name, "Bearer"),
                    JwtParser.ParseClaimsFromJwt(token.Token)
                );

                var userClaimPrincipal = new ClaimsPrincipal(identity);

                var loginUser = new AuthenticationState(userClaimPrincipal);
                User = loginUser.User;

                AppServices.User = User;
                AppServices.JwtAccessToken = token;

                return loginUser;
            }
            catch (Exception ex)
            {
                try
                {
                    await NavigationManager.LocalStorageService.RemoveAsync("token_was");
                }
                catch
                {

                }
            }

            return new AuthenticationState(new ClaimsPrincipal());
        }

        public void Notify()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }

}
