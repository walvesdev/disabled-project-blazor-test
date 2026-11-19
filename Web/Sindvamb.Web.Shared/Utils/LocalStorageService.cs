using Blazor.SubtleCrypto;
using Microsoft.JSInterop;

namespace Sindvamb.Web.Shared.Utils
{
    public interface ILocalStorageService
    {
        Task<T> GetItem<T>(string key);
        Task SetItem<T>(string key, T value);
        Task RemoveItem(string key);
    }

    public class LocalStorageService : ILocalStorageService
    {
        private readonly IJSRuntime _jsRuntime;

        public ICryptoService CryptoService { get; set; }

        public LocalStorageService(IJSRuntime jsRuntime, ICryptoService cryptoService)
        {
            _jsRuntime = jsRuntime;
            CryptoService = cryptoService;
        }

        public async Task<T> GetItem<T>(string key)
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", key);

            if (json == null)
            {
                return default;
            }

            var jsonDecrypted = await CryptoService.DecryptAsync<T>(json);

            return jsonDecrypted;
        }

        public async Task SetItem<T>(string key, T value)
        {
            var valueSerialized = value.Serialize();

            var valueEncrypted = await CryptoService.EncryptAsync(valueSerialized);

            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, valueEncrypted.Value);
        }

        public async Task RemoveItem(string key)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}
