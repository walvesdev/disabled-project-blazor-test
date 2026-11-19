using Sindvamb.Web.Shared.Components;

using System.ComponentModel.DataAnnotations;

namespace Sindvamb.Web.Shared.Models
{
    public class UpdatePasswordDTO
    {

        [Required(ErrorMessage = "A nova senha é obrigatório!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Por favor confirme sua senha!")]
        public string ConfirmPassword { get; set; }

    }
}
