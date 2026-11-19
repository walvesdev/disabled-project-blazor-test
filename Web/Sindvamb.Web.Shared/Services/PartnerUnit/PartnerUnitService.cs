namespace Sindvamb.Web.Shared.Services.PartnerUnit;

public class PartnerUnitService : ServiceBase<PartnerUnitDTO>, IPartnerUnitService
{
    public PartnerUnitService(SwalService swalService, ToastService toastService) : base(toastService, swalService) 
    {
        EntitName = nameof(Domain.Entities.PartnerUnit);
    }

    public async Task<List<PartnerDTO>> GetAllPartners()
    {        
        var service = new ServiceBase<PartnerDTO>(ToastService,SwalService);
        service.EntitName = nameof(Partner);

        return await service.GetAll<PartnerDTO>();
    }


}
