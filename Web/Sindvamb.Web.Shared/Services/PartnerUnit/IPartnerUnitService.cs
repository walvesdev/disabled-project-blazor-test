using Sindvamb.Domain.DTO;
using Sindvamb.Web.Shared.Services;

namespace Sindvamb.Web.Shared.Services.PartnerUnit;

public interface IPartnerUnitService : IServiceBase<PartnerUnitDTO>
{
    Task<List<PartnerDTO>> GetAllPartners();
}
