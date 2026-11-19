using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services.PartnerUnit;

namespace Sindvamb.Web.Shared.Pages.Sistema.PartnersUnits
{
    public partial class ListPartnerUnit : ListComponentBase<PartnerUnitDTO, IPartnerUnitService>
    {
        protected override void OnInitialized()
        {
            base.OnInitialized();

        }
    }
}
