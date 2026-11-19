namespace Sindvamb.Web.Shared.Services.OrderAreas;

public class ProductAreaService : ServiceBase<ProductAreaDTO>, IProductAreaService
{
    public ProductAreaService(SwalService swalService, ToastService toastService) : base(toastService, swalService)
    {
        //SwalService = swalService;
        //ToastService = toastService;
        EntitName = nameof(ProductArea);
    }

}
