using System.ComponentModel.DataAnnotations;

namespace Sindvamb.Domain.DTO
{
    public class RegisterUserDTO
    {

        [Required(ErrorMessage = "Protocol é Obrigatório!")]
        public string Matricula { get; set; }
        
        [Required(ErrorMessage = "Nome é Obrigatório!")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Senha é Obrigatório!")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Email é Obrigatório!")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Cpf/Cnpj é Obrigatório!")]
        public string CpfCnpj { get; set; }
        
        [Required(ErrorMessage = "Selecione sua unidade, é Obrigatório!")]
        public Guid PartnerUnitId { get; set; }
    }
}
