using Microsoft.JSInterop;

namespace Sindvamb.Web.Shared.Utils
{
    public class LocalStorage
    {
        private readonly IJSRuntime _jsRuntime;

        public LocalStorage(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task SetAsync<T>(string key, T value)
        {
            string jsVal = null;
            if (value != null)
                jsVal = value.Serialize();
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem",
                new object[] { key, jsVal.ToBase64String() });
        }
        public async Task<T?> GetAsync<T>(string key)
        {
            string val = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);
            if (val == null) return default;
            T result = val.ToStringFromBase64().Deserialize<T>();
            return result;
        }
        public async Task RemoveAsync(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
        public async Task ClearAsync()
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.clear");
        }
    }
}
