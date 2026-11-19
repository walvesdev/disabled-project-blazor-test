namespace Sindvamb.Web.Shared.Services.AccessControls;

public interface IAccessControlService : IServiceBase<AccessControlDTO>
{
    Task SaveConnectionStateAsync(AuthenticationState? authState);
}
