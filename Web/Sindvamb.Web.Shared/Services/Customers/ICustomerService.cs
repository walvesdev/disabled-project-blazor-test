using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Models;
using Sindvamb.Web.Shared.Services;
using Sindvamb.Web.Shared.Models;

namespace Sindvamb.Web.Shared.Services.Customers;

public interface ICustomerService : IServiceBase<CustomerModel>
{
    Task<List<CustomerTypeDTO>> GetAllCustomerTypes();

    Task<CustomerDTO> GetFullCustomerComand(Guid id);

    Task<bool> CreateDependent(DependentDTO dto);

    Task<bool> DeleteDependent(Guid id);
}
