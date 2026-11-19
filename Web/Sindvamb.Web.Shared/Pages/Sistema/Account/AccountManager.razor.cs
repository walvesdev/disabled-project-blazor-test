using Sindvamb.Web.Shared.Components;
using Sindvamb.Web.Shared.Components.Sistema;
using Sindvamb.Web.Shared.Helpers;
using Sindvamb.Web.Shared.Services.FileControls;
using Console = System.Console;
using Size = BootstrapBlazor.Components.Size;

namespace Sindvamb.Web.Shared.Pages.Sistema.Account
{
    [TabItemOption(Icon = "fa-solid fa-font-awesome", Closable = true, Text = "Minha Conta")]
    public partial class AccountManager : AppComponentBase
    {
        [Inject] IJSRuntime JS { get; set; }

        public string? UserName { get; set; }

        public string? Email { get; set; }

        public Guid? UserId { get; set; }

        public bool ShowCancelButton { get; set; } = false;

        public string AvatarUrl { get; set; }

        public CustomerDTO? Model { get; set; } = new();

        public UpdatePasswordDTO UpdatePassword { get; set; } = new();
        public bool ShowBody { get; set; } = false;

        protected override async Task OnParametersSetAsync()
        {
            UserId = AppServices.UserId;

            await UpdateCustomerModel();


        }

        private async Task ShowDialog()
        {
            try
            {
                var option = new DialogOption
                {
                    Title = $"Alterar Senha de {UserName}",
                    Size = Size.Medium,
                    IsBackdrop = true,
                    IsScrolling = true,
                    ShowFooter = false,
                    OnCloseAsync = async () => await Save()
                };

                option.BodyTemplate = BootstrapDynamicComponent.CreateComponent<UpdatePasswordComponent>(
                    new Dictionary<string, object?>
                    {
                        ["Model"] = UpdatePassword,
                        ["OnCloseAsync"] = new Action(() => option.CloseDialogAsync())
                    }).Render();

                await DialogService.Show(option);
            }
            catch (Exception e)
            {
                await SwalService.Show(new SwalOption()
                {
                    Category = SwalCategory.Error,
                    Content = "Erro ao atualizar senha!",
                });
            }
        }

        private async Task Save()
        {
            if (UpdatePassword.Password.IsNull() &&
                UpdatePassword.ConfirmPassword.IsNull()
               )
            {
                return;
            }


            if (
                UpdatePassword.Password.IsNotNull() &&
                UpdatePassword.ConfirmPassword.IsNotNull() &&
                !UpdatePassword.Password.Equals(UpdatePassword.ConfirmPassword)
            )
            {
                await SwalService.Show(new SwalOption()
                {
                    Category = SwalCategory.Error,
                    Content = "Verifique as senhas digitadas, as senhas novas não são iguais!",
                });

                return;
            }

            Model.User.Password = UpdatePassword.Password;

            var sucesso = await UserService.Update(Model.User);

            if (sucesso)
            {
                await SwalService.Show(new SwalOption()
                {
                    Category = SwalCategory.Success,
                    Content = "A senha foi alterada com sucesso!",
                    OnCloseAsync = new(() => NavigationManager.NavigateTo("/Account/Logout"))
                });
            }
            else
            {
                await SwalService.Show(new SwalOption()
                {
                    Category = SwalCategory.Error,
                    Content = "Erro ao alterar a senha, por favor contate o suporte!",
                });
            }
        }

        private async Task UpdateCustomerModel()
        {

            var dto = await CustomerService.GetFullCustomerComand(AppServices.UserId);
            Model = dto?.MapTo<CustomerDTO>();

            try
            {
                var avatar = await FileControlService.Get<FileControlDTO>(Model.User.FileControl.FirstOrDefault(f => f.Description!.Contains("3X4")).Id);
                AvatarUrl = ImageHelper.GetImageBase64(avatar?.FileArray!);
            }
            catch
            {
                //
            }


            ShowBody = true;
            
        }

        private async Task Update()
        {
            try
            {
                var canSave = await SwalService.ShowModal(new SwalOption()
                {
                    Category = SwalCategory.Question,
                    Content = "Confirma as alterações em suas informações?",
                    OnCloseAsync = OnCloseUpdateCustomerModel,
                    ConfirmButtonText = "Salvar",
                    CloseButtonText = "Cancelar",
                    ShowClose = true
                });

                if (canSave)
                {
                    var isSuccesss = await CustomerService.Update(Model);

                    if (isSuccesss)
                    {
                        await SwalService.Show(new SwalOption()
                        {
                            Category = SwalCategory.Success,
                            Content = "Informações alteradas com sucesso!",
                            OnCloseAsync = UpdateCustomerModel
                        });
                    }
                    else
                    {
                        await SwalService.Show(new SwalOption()
                        {
                            Category = SwalCategory.Error,
                            Content = "Erro ao alterar informações, por favor contate o suporte!",
                            OnCloseAsync = UpdateCustomerModel
                        });
                    }
                }

            }
            catch (Exception e)
            {
                // ignore
            }

            ShowCancelButton = false;
            await InvokeAsync(StateHasChanged);
        }

        private async Task OnCloseUpdateCustomerModel()
        {
            await NavigationManager.NavigateTo("/home");
        }

        private async Task CancelUpdate()
        {
            var canSave = await SwalService.ShowModal(new SwalOption()
            {
                Category = SwalCategory.Question,
                Content = "Deseja cancelar as alterações em suas informações?",
                ConfirmButtonText = "Sim",
                CloseButtonText = "Não",
                ShowClose = true
            });

            if (canSave)
            {
                ShowCancelButton = false;
                await UpdateCustomerModel();
            }
        }
    }
}