using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Models;
using Sindvamb.Web.Shared.Services;

namespace Sindvamb.Web.Shared.Services.Users;

public interface IUserService : IServiceBase<UserDTO>
{
    
    Task<bool> Register(RegisterUserDTO dto);
    Task<List<UserStatusDTO>> GetAllUserStatus();
    Task<List<RoleDTO>> GetAllRoles();
    Task<List<PartnerUnitDTO>> GetAllPartnerUnit();
    Task<bool> UpdatePassword(ChangePasswordModel dto);
    Task<CustomerModel> GetByCpfCnpj(string cpfCnpj);
    Task<bool> RequestLinkUpdatePasswordCommand(string cpfCnpj);
}
