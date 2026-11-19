
using BootstrapBlazor.Components;

namespace Sindvamb.Web.Shared.Components.Core;

public abstract class AppComponentBase : ComponentBase //, IDisposable, IAsyncDisposable
{
    protected AppComponentBase()
    {
    }


    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object> AdditionalAttributes { get; set; } = default!;

    [Parameter] public string? Class { get; set; }

    protected ElementReference Element { get; set; }

    protected bool IsRenderComplete { get; set; }

    [Inject] protected IJSRuntime JSRuntime { get; set; } = default!;

    [Parameter] public string? Style { get; set; }

    [Inject] protected DialogService DialogService { get; set; }

    [Inject] protected NotificationService NotificationService { get; set; }

    [Inject] protected NavigationManagerService NavigationManager { get; set; }

    [Inject] protected AuthenticationStateProvider? AuthenticationProvider { get; set; }

    [Inject] protected LocalStorage LocalStorageService { get; set; }

    [Inject] protected BrowserService BrowserService { get; set; }

    [Parameter] public RenderFragment ChildContent { get; set; }

    protected bool ShowForm { get; set; }

    protected bool ShowModal { get; set; }

    protected bool IsLoading { get; set; }

    protected bool Disabled { get; set; }


    protected void ToggleLoading()
    {
        IsLoading = !IsLoading;
        
    }

    protected void ToggleShowForm()
    {
        ShowForm = !ShowForm;
        
    }

    protected void ToggleShowModal()
    {
        ShowModal = !ShowModal;
        
    }

    protected void ToggleDisabled()
    {
        Disabled = !Disabled;
        
    }

    //~AppComponentBase()
    //{
    //    Dispose();
    //}

    //public void Dispose()
    //{

    //}

    //public async ValueTask DisposeAsync()
    //{
    //    Dispose();
    //}
}
