
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Models;
using Sindvamb.Web.Shared.Services;

namespace Sindvamb.Web.Shared.Services.Users;

public interface IContactService : IServiceBase<ContactDTO>
{
    // Task<List<ContactDTO>> GetAllContacts();
}
