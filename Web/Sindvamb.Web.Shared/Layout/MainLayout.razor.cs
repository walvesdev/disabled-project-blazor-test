using BootstrapBlazor.Components;

using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Sindvamb.Domain.Entities;

using Sindvamb.Web.Shared.Services.Users;

using Sindvamb.Web.Shared.Utils;

using System.Security.Claims;
using Sindvamb.Web.Shared.Extensions;
using Sindvamb.Web.Shared.Helpers;

namespace Sindvamb.Web.Shared.Layout
{
    public partial class MainLayout
    {
        private bool UseTabSet { get; set; } = true;

        private string Theme { get; set; } = "";

        private bool IsOpen { get; set; }

        private bool IsFixedHeader { get; set; } = true;

        private bool IsFixedFooter { get; set; } = true;

        private bool IsFullSide { get; set; } = false;

        private bool ShowFooter { get; set; } = true;

        private List<MenuItem>? Menus { get; set; }

        [Inject] public NavigationManagerService NavigationManager { get; set; }

        [Inject] public SwalService SwalService { get; set; }

        [Inject] public IUserService UserService { get; set; }

        public string? ReturnUrl { get; set; } = null;

        private string FaIcon(string icon) => $"<i class=\"fa-solid fa-{icon}\"></i>";

        [CascadingParameter] private Task<AuthenticationState>? authenticationState { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string AvatarUrl { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (authenticationState is not null)
            {

                var authState = await authenticationState;
                var authStateUser = authState?.User;
                var id = authStateUser?.FindFirst("Sub")?.Value;

                var name = authStateUser?.Identity?.Name?.Split(' ');

                if (!name.IsNullOrEmpty())
                {
                    UserName = name.Length == 1 ? $"{name[0]}" : name.Length >= 2 ? $"{name[0]} {name[1]}" : "Usuário";
                }

                Menus = GetIconSideMenuItems(authStateUser);

                //await InvokeAsync(StateHasChanged);

                try
                {
                    if (!id.IsNullOrWhiteSpace())
                    {
                        var user = await UserService.Get<User>(id);

                        if (user.IsNotNull())
                        {
                            AvatarUrl = ImageHelper.GetImageBase64(user.FileControl.FirstOrDefault(f => f.Description!.Contains("3X4"))?.FileArray!);
                        }
                    }
                }
                catch
                {
                    //
                }
            }


        }

        private static List<MenuItem> GetIconSideMenuItems(ClaimsPrincipal? user)
        {
            var home = new MenuItem { Text = "Início", Icon = "fa-solid fa-home text-primary", Url = "/home", Match = NavLinkMatch.All };
            var dashboard = new MenuItem { Text = "Dashboard", Icon = "fa-solid fa-dashboard text-primary", Url = "/dashboard", Match = NavLinkMatch.All, IsDisabled = AppServices.IsCustomer() };
            var dashboardCustomer = new MenuItem { Text = "Dashboard", Icon = "fa-solid fa-dashboard text-primary", Url = "/dashboard-customer", Match = NavLinkMatch.All, };
            var relatorio = new MenuItem { Text = "Relatórios", Icon = $"fa-solid fa-file-medical-alt text-primary", Url = "/reports", IsDisabled = AppServices.IsCustomer(), IsActive = !AppServices.IsCustomer() };

            var configuracoes = new MenuItem
            {
                Text = "Configurações",
                Icon = "fas fa-toolbox text-primary",
                IsDisabled = AppServices.IsCustomer(),
                IsActive = !AppServices.IsCustomer(),
                Items = new List<MenuItem>
                {
                    new() { Text = "Clientes", Icon = "far fa-address-card text-primary", Url = "/customer" },
                    new() { Text = "Demanda", Icon = "fab fa-stack-overflow text-primary", Url = "/order" },
                    new() { Text = "Parceiros", Icon = "far fa-handshake text-primary", Url = "/partner" },
                    new() { Text = "Produtos", Icon = "fab fa-product-hunt text-primary", Url = "/product-area" },
                    new() { Text = "Solicitações", Icon = "fas fa-tasks text-primary", Url = "/requests" },
                    new() { Text = "Usuários", Icon = "fa-solid fa-user-cog text-primary", Url = "/users" },
                },
            };

            var menus = new List<MenuItem>();
            menus.Add(home);

            if (AppServices.IsAdmin() || AppServices.IsMaster())
            {

                menus.Add(dashboard);
                menus.Add(relatorio);
                menus.Add(configuracoes);
            }

            if (AppServices.IsCustomer())
            {
                //menus.Add(new MenuItem { Text = "Dashboard", Icon = "fa-solid fa-dashboard text-primary", Url = "/dashboard", Match = NavLinkMatch.All });
                menus.Add(dashboardCustomer);
            }

            return menus;
        }

        private async Task OnErrorHandleAsync(ILogger logger, Exception exception)
        {

            await SwalService.Show(new SwalOption()
            {
                Category = SwalCategory.Error,
                Title = "Erro ao processar requisição",
                Content = exception.Message,
            });

            logger.Log(LogLevel.Error, exception.Message);
            System.Console.WriteLine(exception.Message);
        }


    }
}
