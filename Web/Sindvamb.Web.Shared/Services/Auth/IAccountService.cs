using Sindvamb.Domain.DTO;

namespace Sindvamb.Web.Shared.Services.Auth
{
    public interface IAccountService
    {

        Task Login(LoginDTO model);
        Task Recovery(string model);
        Task UpdatePassword(string senha, string securityCode);
        Task Logout();
        //Task Register(AddUser model);
        //Task<IList<LoginDTO>> GetAll();
        //Task<LoginDTO> GetById(string id);

    }
}
