using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services.PartnerUnit;

namespace Sindvamb.Web.Shared.Pages.Sistema.PartnersUnits
{
    public partial class AddEditPartnerUnit : EditComponentBase<PartnerUnitDTO, IPartnerUnitService, Guid>
    {


        public List<PartnerDTO> Partners { get; set; } = new();

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            OperationEntityText = "Unidade";

            Partners = await Service.GetAllPartners();
        }

        private async Task OnPartnerChanged(SelectedItem item)
        {
            Item.Partner = Partners.FirstOrDefault(c => c.Id.Equals(Guid.Parse(item.Value)))!;
        }
    }
}
