using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sindvamb.Domain.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "CPF ou CNPJ é obrigatório!")]
        public string CpfCnpj { get; set; }

        [Required(ErrorMessage = "A Senha é obrigatória!")]
        public string Password { get; set; }
        public bool Remember { get; set; }
        
    }
    
    public class RecoveryDTO
    {
        [Required(ErrorMessage = "{0} é obrigatório!")]
        public string Email { get; set; }

    }    
   
}
