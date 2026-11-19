namespace Sindvamb.Templates.Pages.Usuarios
{
    public class RecoveryPasswordRequest : BasePage
    {
        public string Email { get; set; }
        public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        public string Protocolo { get; set; }
        public string SecurityCode { get; set; }
    }
}