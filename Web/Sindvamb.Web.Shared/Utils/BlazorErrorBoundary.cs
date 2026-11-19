using BootstrapBlazor.Components;

using Microsoft.AspNetCore.Components.Rendering;

using Sindvamb.Web.Shared.Extensions;

namespace Sindvamb.Web.Shared.Utils;

public class BlazorErrorBoundary : ErrorBoundaryBase
{
    [Inject]
    [NotNull]
    private ILogger<BlazorErrorBoundary>? Logger { get; set; }

    [Inject]
    [NotNull]
    private IConfiguration? Configuration { get; set; }

    [Inject]
    [NotNull]
    private ToastService? ToastService { get; set; }

    [Parameter]
    public Func<ILogger, Exception, Task>? OnErrorHandleAsync { get; set; }

    [Parameter]
    public bool ShowToast { get; set; } = true;

    [Parameter]
    public bool ShowErrorDetails  { get; set; } = true;

    [Parameter]
    [NotNull]
    public string? ToastTitle { get; set; }

    protected override async Task OnErrorAsync(Exception exception)
    {
        if (OnErrorHandleAsync != null)
        {
            await OnErrorHandleAsync(Logger, exception);
        }
        else
        {
            if (ShowToast)
            {
                await ToastService.Error(ToastTitle, exception.Message);
            }

            Logger.LogError(exception, "{BootstrapBlazorErrorBoundary} {OnErrorAsync} log this error", nameof(BlazorErrorBoundary), nameof(OnErrorAsync));
        }
    }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        if (OnErrorHandleAsync == null)
        {
            var ex = CurrentException ?? _exception;
            if (ex != null)
            {
                _exception = null;
                builder.AddContent(0, ExceptionContent(ex));
            }
        }
        builder.AddContent(1, ChildContent);
    }

    private Exception? _exception = null;

    private RenderFragment<Exception> ExceptionContent => ex => builder =>
    {
        if (ErrorContent != null)
        {
            builder.AddContent(0, ErrorContent(ex));
        }
        else
        {
            var index = 0;
            builder.OpenElement(index++, "div");
            builder.AddAttribute(index++, "class", "error-stack");
            builder.AddContent(index++, GetErrorContentMarkupString(ex));
            builder.CloseElement();
        }
    };

    private MarkupString GetErrorContentMarkupString(Exception ex)
    {
        //ShowErrorDetails ??= Configuration.GetValue("DetailedErrors", false);
        return ShowErrorDetails  is true
            ? ex.FormatMarkupString(Configuration.GetEnvironmentInformation())
            : new MarkupString(ex.Message);
    }

    public async Task RenderException(Exception exception, IHandlerException? handler)
    {
        await OnErrorAsync(exception);

        if (handler != null)
        {
            await handler.HandlerException(exception, ExceptionContent);
        }
        else
        {
            _exception = exception;
            
        }
    }
    //public ObservableCollection<Exception> BlazorExceptions = new();

    //public object NotificationService { get; set; }
    //[Inject] public NavigationManagerService NavigationService { get; set; }
    //public object DialogService { get; set; }
    //[Inject] public ILogger Logger { get; set; }


    //protected override Task OnErrorAsync(Exception exception)
    //{

    //    Console.WriteLine(exception.Serialize());

    //    // if (exception is CustomException)
    //    // {
    //    //     NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Detail = exception.Message, Duration = 5000 });
    //    // }
    //    // else
    //    // {
    //    //     NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Detail = "Erro ao processar requisição, por favor entre em contato com suporte técnico!", Duration = 5000 });
    //    // }
    //    //
    //    // NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Warning, Detail = "Você será redirecionado para pagina inicial da aplicação!", Duration = 5000 });

    //    Task.Delay(5000);

    //    //NavigationService.NavigateTo($"/");

    //    return Task.CompletedTask;
    //}

    //protected override void BuildRenderTree(RenderTreeBuilder builder)
    //{
    //    Recover();

    //    if (CurrentException is null)
    //    {
    //        builder.AddContent(0, ChildContent);
    //        base.BuildRenderTree(builder);
    //    }
    //    else
    //    {
    //        builder.AddContent(1, ErrorContent(CurrentException));
    //    }

    //}
}

