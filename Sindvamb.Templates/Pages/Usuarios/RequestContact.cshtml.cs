namespace Sindvamb.Templates.Pages.Usuarios
{
    public class RequestContact : BasePage
    {
        public string Name { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Profile { get; set; }
        public string Message { get; set; }
        public string? Response { get; set; }
        public bool HasViewd { get; set; }
        public bool Answered { get; set; }
        public DateTime? AnsweredDate { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}