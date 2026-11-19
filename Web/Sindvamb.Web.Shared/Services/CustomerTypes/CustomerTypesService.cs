namespace Sindvamb.Web.Shared.Services.CustomerTypes;

public class CustomerTypeService : ServiceBase<CustomerTypeDTO>, ICustomerTypeService
{
    public CustomerTypeService(SwalService swalService, ToastService toastService) : base(toastService, swalService) 
    {
        //SwalService = swalService;
        //ToastService = toastService;
        EntitName = nameof(CustomerType);
    }

}
