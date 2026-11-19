namespace Sindvamb.Domain.DTO;

public class PartnerDTO
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string CpfCnpj { get; set; }

    public string Email { get; set; }

    public bool Enabled { get; set; }

    public List<PartnerUnitDTO>? PartnerUnits { get; set; }
}
