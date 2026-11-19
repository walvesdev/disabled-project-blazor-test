

using Microsoft.JSInterop;

namespace Sindvamb.Web.Shared.Extensions;

public static class NavigationManagerExtensions
{
    public static async void NavigateToNewTab(this IJSRuntime JsRuntime, string url)
    {
        if (JsRuntime != null) await JsRuntime.InvokeAsync<object>("open", new object[2] { url, "_blank" });
    }
}
