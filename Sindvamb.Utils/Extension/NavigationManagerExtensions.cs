#region

using Microsoft.JSInterop;

#endregion

namespace Sindvamb.Utils.Extension;

public static class NavigationManagerExtensions
{
    public static async void NavigateToNewTab(this IJSRuntime JsRuntime, string url)
    {
        if (JsRuntime != null) await JsRuntime.InvokeAsync<object>("open", new object[2] { url, "_blank" });
    }
}
