using System.ComponentModel.DataAnnotations;

namespace Sindvamb.Domain.Models;

public class ChangePasswordModel
{    
    public string CpfCnpj { get; set; }
        
    [Required(ErrorMessage = "Senha é Obrigatório!")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "Confirme a senha!")]
    public string ConfirmPassword { get; set; }
    
    public string SecurityCode { get; set; }
        
}