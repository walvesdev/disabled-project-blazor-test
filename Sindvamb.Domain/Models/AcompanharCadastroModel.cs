using System.ComponentModel.DataAnnotations;

namespace Sindvamb.Domain.Models
{
    public class AcompanharCadastroModel
    {
        [Required(ErrorMessage = "Por favor insira  seu CPF")]
        public string CpfCnpj { get; set; }
    }
}
