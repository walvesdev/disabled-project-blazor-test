using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services.Partners;

namespace Sindvamb.Web.Shared.Pages.Sistema.Partners
{
    public partial class ListPartner : ListComponentBase<PartnerDTO, IPartnerService>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
    }
}
