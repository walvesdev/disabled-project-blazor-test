using Sindvamb.Domain.Entities;

namespace Sindvamb.Domain.DTO;

public class RegistrationRequestDTO
{
    public virtual Guid UserId { get; set; }
    public virtual UserDTO User { get; set; } = new UserDTO();
    public bool? Approved { get; set; }
    public string Reason { get; set; }
    public string Protocol { get; set; }
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public ICollection<DependentDTO> Dependents { get; set; } = new List<DependentDTO>();
}
