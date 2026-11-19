using Sindvamb.Utils.Extension;
using Sindvamb.Utils.Utils;

namespace Sindvamb.Templates.Pages.Usuarios
{
    public class DeclaracaoSesc : BasePage
    {
        public DeclaracaoSesc()
        {
            // No Docker se não tiver permissão de escrita no diretório usar sempre FileMode.Open, FileAccess.Read, FileShare.Read
            // senão receberá erro de permissão UnauthorizedAccessException 
            var img = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "timbrado.png");
            using var f = new FileStream(img, FileMode.Open, FileAccess.Read, FileShare.Read);
            var arr = f.GetAllBytes();
            BG = ImageHelper.GetImageBase64(arr);
        }
        public string Name { get; set; }
        public string BG { get; set; }
    }
}