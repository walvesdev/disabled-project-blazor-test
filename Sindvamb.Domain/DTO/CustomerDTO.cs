namespace Sindvamb.Domain.DTO;

public class CustomerDTO
{
    public Guid Id { get; set; }

    public CustomerDTO()
    {
        CustomerOrder = new HashSet<CustomerOrderDTO>();
        Dependents = new HashSet<DependentDTO>(); 
    }

    public bool IsRegistrationRequest { get; set; }

    public virtual Guid UserId { get; set; }

    public virtual UserDTO User { get; set; } = new();

    public virtual Guid? CustomerTypeId { get; set; }

    public virtual CustomerTypeDTO CustomerType { get; set; } = new();

    public virtual ICollection<DependentDTO> Dependents { get; set; }

    public virtual ICollection<CustomerOrderDTO> CustomerOrder { get; set; }
}
