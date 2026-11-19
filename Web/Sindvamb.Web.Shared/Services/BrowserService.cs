using Microsoft.JSInterop;


namespace Sindvamb.Web.Shared.Services
{
    public class BrowserService
    {
        private readonly IJSRuntime _js;
        public IToggleSpinnerEvent ToggleSpinnerEvent { get; set; }
        public bool IsLoading { get; set; }

        public BrowserService(
            IJSRuntime js,
            IToggleSpinnerEvent toggleSpinnerEvent
            )
        {
            _js = js;
            ToggleSpinnerEvent = toggleSpinnerEvent;
            ToggleSpinnerEvent.Toggle += ToggleSpinnerChange;
        }

        public async Task<BrowserDimension> GetDimensions()
        {
            return await _js.InvokeAsync<BrowserDimension>("getDimensions");
        }

        private void ToggleSpinnerChange()
        {
            IsLoading = !IsLoading;
        }
        
        public void ToggleSpinner()
        {
            ToggleSpinnerEvent.CallToggleSpinner();
        }

    }

    public class BrowserDimension
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
