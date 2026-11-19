using Blazorise;
using Blazorise.Bootstrap5;

using BootstrapBlazor.Components;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Sindvamb.Web;
using Sindvamb.Web.Shared.Extensions;

using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.SetupBlazorBuilder(builder.Configuration);


builder.Services.AddCascadingAuthenticationState();
builder.Services.AddAuthenticationStateDeserialization();

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers();

builder.Services.AddBootstrapBlazor();

builder.Services.Configure<BootstrapBlazorOptions>(op =>
{
    op.ToastDelay = 4000;
    op.SupportedCultures = new List<string> { "zh-CN", "pt-BR", "en-US" };
});

var host = builder.Build();

var cultureName = "pt-BR";
var culture = new CultureInfo(cultureName);
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

await host.RunAsync();


