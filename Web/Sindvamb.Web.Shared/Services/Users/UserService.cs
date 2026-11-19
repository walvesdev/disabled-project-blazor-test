namespace Sindvamb.Web.Shared.Services.Users;

public class UserService : ServiceBase<UserDTO>, IUserService
{

    public UserService(SwalService swalService, ToastService toastService) : base(toastService, swalService)
    {
        EntitName = nameof(User);
    }

    public async Task<bool> Register(RegisterUserDTO dto)
    {
        var http = new HttpClientService<RegisterUserDTO, bool, Guid>("User/Register");

        var result = await http.Post(dto);

        if (!result.IsSuccessStatus)
        {
            var message = result.TryGetDomainNotification("Erro ao cadastrar usuário. Por favor contate o suporte!");

             await ShowErrorModal(message);
        }

        return result.Data;
    }
 
    public async Task<List<UserStatusDTO>> GetAllUserStatus()
    {
        var http = new HttpClientService<dynamic, List<UserStatusDTO>, dynamic>($"User/GetAllUserStatus");

        var result = await http.Get();

        if (!result.IsSuccessStatus)
        {
            var message = result.TryGetDomainNotification($"Erro ao buscar Status do Usuario. Por favor contate o suporte!");

             await ShowErrorModal(message);
        }

        return result.Data;
    }
    
    public async Task<List<RoleDTO>> GetAllRoles()
    {
        var http = new HttpClientService<dynamic, List<RoleDTO>, dynamic>($"User/GetAllRoles");

        var result = await http.Get();

        if (!result.IsSuccessStatus)
        {
            var message = result.TryGetDomainNotification($"Erro ao buscar as Permissões do Usuario. Por favor contate o suporte!");

             await ShowErrorModal(message);
        }

        return result.Data;
    }

    public async Task<List<PartnerUnitDTO>> GetAllPartnerUnit()
    {
        var service = new ServiceBase<PartnerUnitDTO>(ToastService,SwalService);
        service.EntitName = nameof(Domain.Entities.PartnerUnit);

        return await service.GetAll<PartnerUnitDTO>();
    }

    public async Task<bool> UpdatePassword(ChangePasswordModel dto)
    {
        var http = new HttpClientService<ChangePasswordModel, bool, Guid>("User/UpdatePassword");

        var result = await http.Post(dto);

        if (!result.IsSuccessStatus)
        {
            var message = result.TryGetDomainNotification("Erro ao alterar a senha do usuário. Por favor contate o suporte!");
            await ShowErrorModal(message);
        }

        return result.Data;
    }

    public async Task<CustomerModel> GetByCpfCnpj(string cpfCnpj)
    {
        var http = new HttpClientService<string, CustomerModel, Guid>($"User/GetByCpfCnpj/{cpfCnpj}");

        var result = await http.Get();

        if (!result.IsSuccessStatus)
        {
            var message = result.TryGetDomainNotification($"Erro ao buscar Usuario. Por favor contate o suporte!");
            await ShowErrorModal(message);
        }

        return result.Data;
    }

    public async Task<bool> RequestLinkUpdatePasswordCommand(string cpfCnpj)
    {
        var http = new HttpClientService<string, bool, Guid>($"User/RequestLinkUpdatePasswordCommand/{cpfCnpj?.GetDigits()}");

        var result = await http.Get();

        if (!result.IsSuccessStatus)
        {
            var message = result.TryGetDomainNotification($"Erro ao solicitar redefinição de senha. Por favor contate o suporte!");
            await ShowErrorModal(message);
        }

        return result.Data;
    }


}
