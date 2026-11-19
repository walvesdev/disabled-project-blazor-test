namespace Sindvamb.Domain.DTO;

public class ContactRequestDTO
{
    public Guid Id { get; set; }
    public string Subject { get; set; }
    public string Profile { get; set; }
    public string Message { get; set; }
    public string? Response { get; set; }
    public bool HasViewd { get; set; }
    public bool HasAnswered { get; set; }
    public bool HasPendding { get; set; }
    public DateTime? AnsweredDate { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public virtual Guid? ContactId { get; set; }
    public virtual ContactDTO? Contact { get; set; } = new();
}