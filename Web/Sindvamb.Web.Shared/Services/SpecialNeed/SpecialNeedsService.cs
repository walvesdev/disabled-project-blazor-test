namespace Sindvamb.Web.Shared.Services.SpecialNeed;

public class SpecialNeedsService : ServiceBase<SpecialNeedsDTO>, ISpecialNeedsService
{
    public SpecialNeedsService(SwalService swalService, ToastService toastService) : base(toastService, swalService) 
    {
        EntitName = nameof(SpecialNeeds);
    }

}
