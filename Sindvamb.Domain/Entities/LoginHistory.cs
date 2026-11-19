

using System.ComponentModel;


using Sindvamb.Domain;

namespace Sindvamb.Domain.Entities
{
    public class LoginHistory : EntityBase<Guid>
    {
        
        public Guid UserId { get; set; }
        public bool IsSuccess { get; set; }
        public string Reason { get; set; }
        public string IpAddress { get; set; }
        public DateTime Date { get; set; }

        public LoginHistory()
        {
            Date = ToFormatedDate(DateTime.UtcNow);
        }

        public DateTime ToFormatedDate(DateTime dateValue)
        {
            try
            {
                const string pattern = "yyyy-MM-dd-HH-mm-ss";
                return DateTime.ParseExact(dateValue.ToString("yyyy-MM-dd-HH-mm-ss"), pattern, null);
            }
            catch
            {
                throw new Exception("Formato de data inválido! ");
            }
        }

    }

    public enum ILoginReason
    {
        [Description("Acesso Autorizado")] Authorized,

        [Description("Acesso Não Autorizado")] NotAuthorized,

        [Description("Falha na Autenticação")] LoginFailure,

        [Description("Usuário Bloqueado")] Blocked,
    }
}
