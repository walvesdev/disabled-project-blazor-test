

using Sindvamb.Domain;



namespace Sindvamb.Domain.Entities
{
    public class EmailHistory : EntityBase<Guid>
    {
        
        public Guid? UserId { get; set; }
        public bool IsSuccess { get; set; }
        public string Reason { get; set; }
        public string Email { get; set; }
        public string TemplateKey { get; set; }
        public string Data { get; set; }
        public string IpAddress { get; set; }
        public string MessageId { get; set; }
        public DateTime Date { get; set; }
    }
}
