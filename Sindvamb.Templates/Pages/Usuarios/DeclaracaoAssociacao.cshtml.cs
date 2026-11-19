using Sindvamb.Utils.Extension;
using Sindvamb.Utils.Utils;

namespace Sindvamb.Templates.Pages.Usuarios
{
    public class DeclaracaoAssociacao : BasePage
    {
        public DeclaracaoAssociacao()
        {
            // No Docker se não tiver permissão de escrita no diretório usar sempre FileMode.Open, FileAccess.Read, FileShare.Read
            // senão receberá erro de permissão UnauthorizedAccessException 
            var img = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "timbrado.png");
            using var f = new FileStream(img, FileMode.Open, FileAccess.Read, FileShare.Read);
            var arr = f.GetAllBytes();
            BG = ImageHelper.GetImageBase64(arr);             

        }

        public string BG { get; set; } 
        public string Name { get; set; }
        public string CpfCnpj { get; set; } 
        public string RG { get; set; } 
        public string Endereco { get; set; }



    }
}