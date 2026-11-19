using Microsoft.AspNetCore.Components;

using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services.Partners;
using Sindvamb.Web.Shared.Services.Users;

namespace Sindvamb.Web.Shared.Pages.Sistema.Partners
{
    public partial class AddEditPartner : EditComponentBase<PartnerDTO, IPartnerService, Guid>
    {
        [Inject] public IUserService UserService { get; set; }

        public int UserStatusCount { get; set; }

        public List<PartnerUnitDTO> PartnerUnitList { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            OperationEntityText = "Parceiro";

            await base.OnInitializedAsync();
            await GetAllPartnerUnit();
        }

        public async Task GetAllPartnerUnit()
        {
            PartnerUnitList = await UserService.GetAllPartnerUnit();
        }
    }
}
