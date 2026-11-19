namespace Sindvamb.Web.Shared.Services.Users
{
    public class UserRegistrationService : ServiceBase<RegistrationRequestDTO>, IUserRegistrationService
    {
        public UserRegistrationService(SwalService swalService, ToastService toastService) : base(toastService, swalService)
        {
            EntitName = nameof(User);
        }

        public async Task<List<RegistrationRequestDTO>> GetAllUnauthorizedUsers()
        {
            var http = new HttpClientService<object, List<RegistrationRequestDTO>, dynamic>("User/GetAllUnauthorizedUsers");
            var result = await http.Get();

            if (!result.IsSuccessStatus)
            {
                var message = result.TryGetDomainNotification("Erro ao buscar usuários. Por favor contate o suporte!");
                await ShowErrorModal(message);
            }

            return result.Data;
        }

        public async Task<bool> ApproveUser(RegistrationRequest dto)
        {
            var http = new HttpClientService<RegistrationRequest, bool, Guid>("User/ApproveUser");

            var result = await http.Post(dto);

            if (!result.IsSuccessStatus)
            {
                var message = result.TryGetDomainNotification("Erro ao atualizar usuário. Por favor contate o suporte!");
                await ShowErrorModal(message);
            }

            return result.Data;
        }
        
        public async Task<RegistrationRequestDTO> GetUnauthorizedUserStatus(AcompanharCadastroModel model)
        {
            var http = new HttpClientService<dynamic, RegistrationRequestDTO, dynamic>($"User/GetUnauthorizedUserStatus");

            var result = await http.Post(model);

            if (!result.IsSuccessStatus)
            {
                var message = result.TryGetDomainNotification($"Erro ao buscar os dados de solicitação de cadastro. Por favor contate o suporte!");

                await ShowErrorModal(message);
            }

            return result.Data;
        }

        public async Task<bool> DenyUser(RegistrationRequestDTO dto)
        {
            var http = new HttpClientService<RegistrationRequestDTO, bool, Guid>("User/DenyUser");

            var result = await http.Post(dto);

            if (!result.IsSuccessStatus)
            {
                var message = result.TryGetDomainNotification("Erro ao reprovar o cadastro do usuário. Por favor contate o suporte!");
                await ShowErrorModal(message);
            }

            return result.Data;
        }

        public static async Task<object> GetUnauthorizedUserStatus(object model)
        {
            throw new NotImplementedException();
        }
    }
}
