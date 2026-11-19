using Sindvamb.Web.Shared.Models;

namespace Sindvamb.Web.Shared.Services.Customers;

public class CustomerService : ServiceBase<CustomerModel>, ICustomerService
{
    public CustomerService(SwalService swalService, ToastService toastService) : base(toastService, swalService)
    {
        EntitName = nameof(Customer);
    }

    public async Task<bool> CreateDependent(DependentDTO dto)
    {
        var http = new HttpClientService<DependentDTO, bool, Guid>($"{EntitName}/CreateDependent");

        var result = await http.Post(dto);

        if (!result.IsSuccessStatus)
        {
            var message = result.TryGetDomainNotification("Erro ao atualizar dependente. Por favor contate o suporte!");
            await ShowErrorModal(message);
        }

        return result.Data;
    }

    public async Task<bool> DeleteDependent(Guid id)
    {
        var http = new HttpClientService<DependentDTO, bool, Guid>($"{EntitName}/DeleteDependent/{id}");

        var result = await http.Delete();

        if (!result.IsSuccessStatus)
        {
            var message = result.TryGetDomainNotification("Erro ao deletar dependente. Por favor contate o suporte!");
            await ShowErrorModal(message);
        }

        return result.Data;
    }

    public async Task<List<CustomerTypeDTO>> GetAllCustomerTypes()
    {
        var service = new ServiceBase<CustomerTypeDTO>(ToastService, SwalService)
        {
            EntitName = nameof(CustomerType)
        };

        return await service.GetAll<CustomerTypeDTO>();
    }

    public async Task<CustomerDTO> GetFullCustomerComand(Guid id)
    {
        var http = new HttpClientService<Guid, CustomerDTO, Guid>($"{EntitName}/GetFullCustomerComand/{id}");

        var result = await http.Get();

        if (!result.IsSuccessStatus)
        {
            var message = base.GertErrorMessage(result?.ErrorMessage);
            await ShowErrorModal(message);
        }

        return result.Data;
    }
}
