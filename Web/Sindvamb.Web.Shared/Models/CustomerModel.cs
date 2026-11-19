using Sindvamb.Domain.DTO;

using System.ComponentModel.DataAnnotations;
using Sindvamb.Domain.Models;
using Sindvamb.Web.Shared.Models;

namespace Sindvamb.Web.Shared.Models
{
    public class CustomerModel
    {
        public Guid Id { get; set; }

        public CustomerModel()
        {
            CustomerOrder = new HashSet<CustomerOrderDTO>();
            Dependents = new HashSet<DependentDocumentModel>();
        }

        public bool IsRegistrationRequest { get; set; }

        public virtual Guid UserId { get; set; }

        //[ValidateComplexType]
        public virtual UserDTO User { get; set; } = new();

        public virtual Guid? CustomerTypeId { get; set; }

        public virtual CustomerTypeDTO CustomerType { get; set; } = new();

        public virtual ICollection<DependentDocumentModel> Dependents { get; set; }

        public virtual ICollection<CustomerOrderDTO> CustomerOrder { get; set; }
    }
}
