using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Sindvamb.Web.Shared.Utils.CustomExceptions;

namespace Sindvamb.Web.Shared.Services.Auth
{
    public class AccountService : IAccountService
    {
        public AuthenticationStateProvider _customAuthenticationProvider;

        public LocalStorage _localStorageService;

        public JwtAccessToken AccessToken { get; set; }

        protected object DialogService { get; set; }

        public NavigationManagerService NavigationManager { get; set; }

        public GenericTokentService TokentService { get; set; }

        public SwalService SwalService { get; set; }


        public AccountService(LocalStorage localStorageService,
            AuthenticationStateProvider customAuthenticationProvider,
            JwtAccessToken accessToken,
            NavigationManagerService navigationManager,
            StateContainer stateContainer,
            GenericTokentService tokentService, SwalService swalService)
        {
            _localStorageService = localStorageService;
            _customAuthenticationProvider = customAuthenticationProvider;
            AccessToken = accessToken;
            NavigationManager = navigationManager;
            TokentService = tokentService;
            SwalService = swalService;
            //DialogService = dialogService;
            //SwalService = swalService;
        }

        public async Task Login(LoginDTO model)
        {
            try
            {
                var http = new HttpClientService<LoginDTO, JwtAccessToken, Guid>("Auth/Login");

                var result = await http.Post(model);

                if (!result.IsSuccessStatus)
                {
                    try
                    {
                        var errMsg = JsonConvert.DeserializeObject<ErrorMessage>(result?.ErrorMessage);
                        await ShowErrorModal(errMsg?.Value?.ValidationErrors[0]?.Validator!);
                    }
                    catch
                    {
                        if (result.IsNotNull() && result.Data?.Authenticated == false)
                        {
                            await ShowErrorModal(result!.Data?.Message ?? "Falha ao autenticar!");
                        }
                        else
                        {
                            await ShowErrorModal("Erro ao autenticar. Por favor contate o suporte!");
                        }
                    }
                }

                await _localStorageService.SetAsync("token_was", result.Data);
                AccessToken = result.Data;

                TokentService.Token = result.Data.Token;
                TokentService.Username = result.Data.Name;
                TokentService.ExpirationDate = Convert.ToDateTime(result.Data.Expiration);
                AppServices.JwtAccessToken = result.Data;

                (_customAuthenticationProvider as CustomAuthenticationProvider).Notify();

                // await NavManager.NavigateTo($"/dashboard");
              
            }
            catch
            {
                return;
            }
        }

        public async Task UpdatePassword(string senha, string securityCode)
        {
            try
            {
                var http = new HttpClientService<string, JwtAccessToken, Guid>($"Auth/UpdatePassword/{securityCode}");

                var result = await http.Post(senha);

                if (!result.IsSuccessStatus)
                {
                    throw new CustomException("Esse link não é mais válido solicite um nova redefinição de senha!");
                }

                // await DialogService.Alert($"Senha Alterada com Sucesso!", "Feito!", new ConfirmOptions() { OkButtonText = "Confirmar" });

                await NavigationManager.NavigateTo($"/Account/Login", true, true);
            }
            catch
            {
                return;
            }
        }

        public async Task Logout()
        {
            await _localStorageService.RemoveAsync("token_was");
            (_customAuthenticationProvider as CustomAuthenticationProvider).Notify();

            await NavigationManager.NavigateTo($"/Account/Login", true, true);
        }

        public async Task Recovery(string email)
        {
            try
            {
                var http = new HttpClientService<string, JwtAccessToken, Guid>("Auth/Recovery");

                var result = await http.Post(email);

                if (!result.IsSuccessStatus)
                {
                    throw new CustomException("Erro ao autenticar. Por favor contate o suporte!");
                }

                // await DialogService.Alert($"Você receberá um email com as instruções para recuperar sua senha!", "Feito!", new ConfirmOptions() { OkButtonText = "Confirmar" });

                await NavigationManager.NavigateTo($"/Account/Login", true, true);
            }
            catch
            {
                return;
            }
        }

        public async Task ShowErrorModal(string message)
        {
            await SwalService.Show(new SwalOption()
            {
                Category = SwalCategory.Error,
                Title = "Atenção!",
                Content = message, 
                
            });
        }
    }
}
