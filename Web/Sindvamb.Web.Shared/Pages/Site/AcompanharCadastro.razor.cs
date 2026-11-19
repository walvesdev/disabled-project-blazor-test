
using BootstrapBlazor.Components;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

using Sindvamb.Web.Shared.Models;
using Sindvamb.Web.Shared.Services.FileControls;

namespace Sindvamb.Web.Shared.Pages.Site;

public partial class AcompanharCadastro : AppComponentBase
{
    [Inject][NotNull] private ToastService? ToastService { get; set; }

    [Inject][NotNull] private IUserRegistrationService UserRegistrationService { get; set; }

    [Inject][NotNull] SwalService SwalService { get; set; }

    [Inject][NotNull] NavigationManager NavigationManager { get; set; }

    [Inject][NotNull] IJSRuntime JS { get; set; }

    [Parameter] public string CpfCnpj { get; set; }

    [Parameter] public bool IsModal { get; set; }

    public AcompanharCadastroModel Model { get; set; } = new();

    [Inject] public IFileControlService FileControlService { get; set; }

    public bool ShowCardStatus { get; set; }

    public RegistrationRequestDTO RegistrationRequest { get; set; }

    private readonly TimelineItem[] AlternateTimelineItems =
    [
        new()
        {
            Color = BootstrapBlazor.Components.Color.Success,
            Content = "Envio de Solicitação",
        },
    new()
        {
            Color = BootstrapBlazor.Components.Color.Info,
            Content = "Em Análise",
        },
    new()
        {
            Color = BootstrapBlazor.Components.Color.None,
            Content = "Concluído",
        }
    ];

    protected override async void OnParametersSet()
    {
        if (!CpfCnpj.IsNullOrWhiteSpace())
        {
            Model.CpfCnpj = CpfCnpj;
            await GetRegistrationRequest();
        }

        await base.OnParametersSetAsync();
    }

    public async Task OnValidSubmit(EditContext ctx)
    {
        await GetRegistrationRequest();
    }

    public async Task OnInvalidSubmit(EditContext ctx)
    {
    }

    public async Task RedirectToHome()
    {
        NavigationManager.NavigateTo("/");
    }

    private async Task DownloadFile(FileControlDTO doc)
    {
        var fileStream = new MemoryStream(doc.FileArray);

        using var streamRef = new DotNetStreamReference(stream: fileStream);

        await JS.InvokeVoidAsync("downloadFileFromStream", doc.FileName, streamRef);
    }

    private async Task GetRegistrationRequest()
    {
        try
        {
            ToggleLoading();

            RegistrationRequest = await UserRegistrationService.GetUnauthorizedUserStatus(Model);

            ToggleLoading();

            if (RegistrationRequest.IsNull())
            {
                await SwalService.Show(new SwalOption()
                {
                    Category = SwalCategory.Warning,
                    Content = "Sua solicitação não foi encontrada!",
                });
            }
            else
            {
                ShowCardStatus = true;
                await InvokeAsync(StateHasChanged);
            }
        }
        catch
        {
            await SwalService.Show(new SwalOption()
            {
                Category = SwalCategory.Error,
                Content = "Erro ao processar requisição!",
            });

        }
    }
    
}