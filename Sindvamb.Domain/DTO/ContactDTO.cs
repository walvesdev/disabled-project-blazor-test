namespace Sindvamb.Domain.DTO;

public class ContactDTO
{
    public Guid Id { get; set; }

    public string? Name { get; set; }

    public string? CpfCnpj { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? CellPhone { get; set; }

    public string? Fax { get; set; }

    public string? MainActivityCode { get; set; }

    public string? EconomicActivity { get; set; }

    public virtual Guid? AddressId { get; set; }
    public virtual AddressDTO? Address { get; set; } = new();
}