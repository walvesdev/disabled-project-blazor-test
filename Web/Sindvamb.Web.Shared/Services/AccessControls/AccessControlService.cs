namespace Sindvamb.Web.Shared.Services.AccessControls;

public class AccessControlService : ServiceBase<AccessControlDTO>, IAccessControlService
{
    private IConnectionService ConnectionService { get; set; }

    private WebClientService WebClientService { get; set; }
    
    public AccessControlDTO AccessControl { get; set; } = new();

    public AccessControlService(IConnectionService connectionService, WebClientService webClientService, SwalService swalService, ToastService toastService) : base(toastService, swalService)
    {
        ConnectionService = connectionService;
        WebClientService = webClientService;
        EntitName = nameof(AccessControl);
    }


    public async Task SaveConnectionStateAsync(AuthenticationState? authState)
    {
        try
        {
            if (authState is not null)
            {
                var authStateUser = authState?.User;
                var id = authStateUser?.FindFirst("Sub")?.Value;

                var name = authStateUser?.Identity?.Name?.Split(' ');

                if (!name.IsNullOrEmpty())
                {
                    var userName = $"{name[0]} {name[1]}" ?? "Visitante";
                    AccessControl.UserName = userName;
                    AccessControl.UserId = Guid.Parse(id);
                }
            }

            AccessControl.Connections = ConnectionService
                .Connections?
                .Sort(["ConnectionTime"])?
                .MapTo<List<ConnectionItemModel>>() ?? [];

            await Create(AccessControl);
        }
        catch (Exception e)
        {
            // ignore
        }
    }
}