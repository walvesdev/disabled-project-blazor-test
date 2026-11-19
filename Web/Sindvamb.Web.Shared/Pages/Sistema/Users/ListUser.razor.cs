using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services.Users;
using Sindvamb.Web.Shared.Pages.Sistema.Users;

namespace Sindvamb.Web.Shared.Pages.Sistema.Users
{
    public partial class ListUser : ListComponentBase<UserDTO, IUserService>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
