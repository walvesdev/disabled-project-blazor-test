using Sindvamb.Domain;
using Sindvamb.Domain.Entities;

namespace Sindvamb.Domain.Entities;

public class RegistrationRequest : EntityBase<Guid>
{
    
    public virtual Guid UserId { get; set; }
    public virtual User User { get; set; }

    public bool? Approved { get; set; }    
   
    public string? Reason { get; set; }

    public string Protocol { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;
}
