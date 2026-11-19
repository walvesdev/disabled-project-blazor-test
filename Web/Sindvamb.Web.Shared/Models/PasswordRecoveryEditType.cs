using System.ComponentModel.DataAnnotations;

namespace Sindvamb.Web.Shared.Models
{
    public class PasswordRecoveryEditType
    {


        [AutoGenerateColumn(
            Order = 1,
            Text = "CPF ou CNPJ",
            PlaceHolder = "Insira seu CPF ou CNPJ")
        ]
        [Required(ErrorMessage = "Insira seu CPF ou CNPJ")]
        public string CpfCnpj { get; set; }
    }
}
