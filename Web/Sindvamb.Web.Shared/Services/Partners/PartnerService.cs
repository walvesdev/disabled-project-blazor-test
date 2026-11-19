namespace Sindvamb.Web.Shared.Services.Partners;

public class PartnerService : ServiceBase<PartnerDTO>, IPartnerService
{
    public PartnerService(SwalService swalService, ToastService toastService) : base(toastService, swalService) 
    {
        EntitName = nameof(Partner);
    }
    
}
