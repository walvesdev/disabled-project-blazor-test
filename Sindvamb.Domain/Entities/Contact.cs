using Newtonsoft.Json;

namespace Sindvamb.Domain.Entities;

public class Contact : EntityBase<Guid>
{
    public string Name { get; set; }
    
    public string? CpfCnpj { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? CellPhone { get; set; }

    public string? Fax { get; set; }

    public string? MainActivityCode { get; set; }

    public string? EconomicActivity { get; set; }

    public virtual Guid? AddressId { get; set; }
    public virtual Address Address { get; set; }
    
}