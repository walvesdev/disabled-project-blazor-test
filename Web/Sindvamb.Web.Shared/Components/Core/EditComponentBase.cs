using Microsoft.AspNetCore.Components.Web;

namespace Sindvamb.Web.Shared.Components.Core;

public class EditComponentBase<TItem, TService, TKey> : AppComponentBase where TService : IServiceBase<TItem> where TItem : new()
{
    [Inject] protected TService Service { get; set; }

    [Inject] protected SwalService SwalService { get; set; }

    [Inject] protected NavigationManagerService NavigationService { get; set; }

    [Parameter] public TKey Id { get; set; }

    protected bool IsEdit { get; set; }

    [Parameter] public TItem Item { get; set; }

    protected string OperationEntityText { get; set; }

    [Parameter] public Action OnCloseAsync { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        try
        {
            if (Item.IsNull() && Id.Equals(default(TKey)))
            {
                IsEdit = false;
                Item = new();
            }
            else
            {
                IsEdit = true;
                Item = await Service.Get<TItem>(Id);
            }

            
        }
        catch (Exception e)
        {

            await NavigationService.NavigateTo($"/");
        }
    }

    protected async Task FormSubmit(EditContext context, string returnPath)
    {
        if (!context.Validate())
        {
            await SwalService.Show(new SwalOption()
            {
                Category = SwalCategory.Warning,
                Content = "Por favor verifique os erro de validação!",
                OnCloseAsync = CloseDialg
            });

            return;
        }

        ToggleLoading();

        if (IsEdit)
        {
            var sucesso = await Service.Update(Item);

            if (sucesso)
            {
                await SwalService.Show(new SwalOption()
                {
                    Category = SwalCategory.Success,
                    Content = "Registro atualizado com sucesso!",
                    OnCloseAsync = CloseDialg
                });
            }
            else
            {
                await SwalService.Show(new SwalOption()
                {
                    Category = SwalCategory.Error,
                    Content = "Erro ao atualizar!",
                    OnCloseAsync = CloseDialg
                });
            }
        }
        else
        {
            var sucesso = await Service.Create(Item);

            if (sucesso.IsNotNull())
            {
                await SwalService.Show(new SwalOption()
                {
                    Category = SwalCategory.Success,
                    Content = "Registro adicionado com sucesso!",
                    OnCloseAsync = CloseDialg
                });
            }
            else
            {
                await SwalService.Show(new SwalOption()
                {
                    Category = SwalCategory.Error,
                    Content = "Erro ao adicionar!",
                    OnCloseAsync = CloseDialg
                });
            }
        }


        ToggleLoading();

        if (!returnPath.IsNullOrWhiteSpace())
        {
            await NavigationManager.NavigateTo(returnPath);
        }

        await CloseDialg();

    }

    protected async Task CancelButtonClick(MouseEventArgs args, string returnPath)
    {
        await NavigationManager.NavigateTo(returnPath);
    }

    protected async Task CloseDialg()
    {
        OnCloseAsync?.Invoke();
        
    }
}
