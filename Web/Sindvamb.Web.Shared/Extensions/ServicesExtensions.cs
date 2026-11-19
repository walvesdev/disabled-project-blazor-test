#region

using System.Net.Http;
using BootstrapBlazor.Components;

using Sindvamb.Domain.Validations;
using Sindvamb.Web.Shared.Components.Validator;
using Sindvamb.Web.Shared.Helpers;
using Sindvamb.Web.Shared.Services.AccessControls;
using Sindvamb.Web.Shared.Services.FileControls;

//using Microsoft.AspNetCore.Http;

#endregion

namespace Sindvamb.Web.Shared.Extensions;

public static class ServicesExtensions
{

    public static void SetupBlazorBuilder(this IServiceCollection services, IConfiguration configuration)
    {
        AddBlazorServices(services);

        AppServices.ServiceCollection = services;
        AppServices.Configuration = configuration;
        AppServices.NavigationManager = AppServices.GetService<NavigationManager>();

        //CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
        //CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");
    }

    public static void AddBlazorServices(IServiceCollection services)
    {
#if STAGING
                            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Staging");
                            AppServices.EnvironmentName = "Staging";
#endif
#if HML
                            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "HML");
                            AppServices.EnvironmentName = "HML";
#endif
#if DEBUG
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
        AppServices.EnvironmentName = "Development";
#endif
#if DOCKER
                            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Docker");
                            AppServices.EnvironmentName = "Docker";
#endif
#if PRODUCTION
                            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
                            AppServices.EnvironmentName = "Production";
#endif
#if RELEASE
                            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
                            AppServices.EnvironmentName = "Production";
#endif



        //services.AddBlazorComponents();

        services.AddScoped<LocalStorage>(); 
        services.AddScoped<FluentValidationValidator>(); 
        services.AddScoped<StateContainer>();
        services.AddScoped<NavigationManagerService>();
        services.AddScoped<BrowserService>(); 
        services.AddScoped<ConsoleLogger>(); 
        services.AddScoped<DashboardService>(); 
        // services.AddScoped<ToastService>();
        // services.AddScoped<SwalService>();

        //Validators

        //services.AddScoped<CustomerModelValidator>(); 
        services.AddScoped<AddressDTOValidator>(); 

        var logger = new LoggerFactory().CreateLogger("AppLog");

        services.AddSingleton(typeof(ILogger), logger);

        //services.AddSecurityModule(Configuration);

        services.TryAdd(ServiceDescriptor.Singleton(typeof(IOptions<>), typeof(OptionsManager<>)));
        services.TryAdd(ServiceDescriptor.Scoped(typeof(IOptionsSnapshot<>), typeof(OptionsManager<>)));
        services.TryAdd(ServiceDescriptor.Singleton(typeof(IOptionsMonitor<>), typeof(OptionsMonitor<>)));

        services
            .AddScoped<AccountService>()
            .AddSingleton(typeof(OptionsManager<>))
            .AddScoped(typeof(GenericTokentService))
            .AddScoped(typeof(BasicAccessToken))
            .AddScoped(typeof(JwtAccessToken))
            .AddScoped(typeof(HttpClientService<,,>))
            .AddScoped<AuthenticationStateProvider, CustomAuthenticationProvider>()
            .AddScoped<IAccessControlService, AccessControlService>()
            .AddScoped<IAccountService, AccountService>()
            .AddScoped<IPartnerService, PartnerService>()
            .AddScoped<IPartnerUnitService, PartnerUnitService>()
            .AddScoped<ICustomerService, CustomerService>()
            .AddScoped<ICustomerTypeService, CustomerTypeService>()
            .AddScoped<IFileControlService, FileControlService>()
            .AddScoped<IOrderTypeService, OrderTypeService>()
            .AddScoped<IProductCategoryService, ProductCategoryService>()
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<IOrderStatusService, OrderStatusService>()
            .AddScoped<IProductAreaService, ProductAreaService>()
            .AddScoped<IContactService, ContactService>()
            .AddScoped<IContactRequestService, ContactRequestService>()
            .AddScoped<ISpecialNeedsService, SpecialNeedsService>()
            .AddScoped<IEducationDegreeService, EducationDegreeService>()
            .AddScoped<IKTThemeHelpers, KTThemeHelpers>()
            .AddScoped<IHttpService, HttpService>()
            .AddScoped<ILocalStorageService, LocalStorageService>()
            .AddScoped<IUserService, UserService>()
            .AddSingleton<IToggleSpinnerEvent, ToggleSpinnerEvent>()
            .AddScoped<IUserRegistrationService, UserRegistrationService>();

        services.AddSubtleCrypto(opt =>
                opt.Key = "wastec2020041090" //Use another key
        );

        services.AddCascadingAuthenticationState();

        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy(AuthRole.ADMIN, policy => policy.RequireRole(AuthRole.ADMIN));
            options.AddPolicy(AuthRole.MASTER, policy => policy.RequireRole(AuthRole.MASTER));
            options.AddPolicy(AuthRole.SUPPORT, policy => policy.RequireRole(AuthRole.SUPPORT));
            options.AddPolicy(AuthRole.CUSTOMER, policy => policy.RequireRole(AuthRole.CUSTOMER));
        });


    }
}
