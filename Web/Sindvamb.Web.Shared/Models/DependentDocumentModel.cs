using System.ComponentModel.DataAnnotations;
using Sindvamb.Domain.Models;

namespace Sindvamb.Web.Shared.Models
{
    public class DependentDocumentModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string CpfCnpj { get; set; }

        public string Email { get; set; }

        public string CellPhone { get; set; }

        public string Type { get; set; }

        public virtual Guid CustomerId { get; set; }

        public virtual Guid UserId { get; set; }

        public List<FileControlDTO> FileControl { get; set; } = new();


        [Required(ErrorMessage = "Por favor insira a Foto 3X4!")]
        public IBrowserFile Foto { get; set; }

        [Required(ErrorMessage = "Por favor insira o RG!")]
        public IBrowserFile Rg { get; set; }

        [Required(ErrorMessage = "Por favor insira comprovante de residência!")]
        public IBrowserFile Residencia { get; set; }

        public IBrowserFile CpfCnpjFile { get; set; }

        public IBrowserFile Certidao { get; set; }

        public IBrowserFile Parentesco { get; set; }

        public IBrowserFile Escolaridade { get; set; }

        public IBrowserFile Juizado { get; set; }
    }
}
