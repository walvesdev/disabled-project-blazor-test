using System.ComponentModel.DataAnnotations;
using Sindvamb.Domain.Models;

namespace Sindvamb.Web.Shared.Models
{
    public class HolderDocumentModel
    {
        [Required(ErrorMessage= "Selecione sua foto 3X4")]
        public IBrowserFile Foto { get; set; }

        [Required(ErrorMessage= "Selecione sua foto do CPF")]
        public IBrowserFile Rg { get; set; }

        [Required(ErrorMessage= "Selecione sua foto do RG")]
        public IBrowserFile CpfCnpj { get; set; }

        public IBrowserFile Certidao { get; set; }

        [Required(ErrorMessage= "Comprovante de residência")]
        public IBrowserFile Residencia { get; set; }

        [Required(ErrorMessage = "Porfavor insira o Comprovante de Pagamento")]
        public IBrowserFile ComprovantePagamento { get; set; }
    }
}
