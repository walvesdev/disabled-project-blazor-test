using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Domain.Models;
using Sindvamb.Web.Shared.Services;

namespace Sindvamb.Web.Shared.Services.Users
{
    public interface IUserRegistrationService : IServiceBase<RegistrationRequestDTO>
    {
        Task<List<RegistrationRequestDTO>> GetAllUnauthorizedUsers();
        Task<bool> ApproveUser(RegistrationRequest dto);
        
        Task<RegistrationRequestDTO> GetUnauthorizedUserStatus(AcompanharCadastroModel model);
        Task<bool> DenyUser(RegistrationRequestDTO dto);
    }
}
