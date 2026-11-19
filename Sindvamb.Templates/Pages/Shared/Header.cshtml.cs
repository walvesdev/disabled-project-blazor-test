using Sindvamb.Utils.Configurations;
using Sindvamb.Utils.Helpers;

using System.Text;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;
using Sindvamb.Utils.Utils;

namespace Sindvamb.Templates.Pages.Shared
{
    public class Header : BasePage
    {
        public string Logo { get; set; }

        public Header()
        {
            var logo = $"https://www.sindvamb.com.br/img/logo.png";

            //var directory = AppDomain.CurrentDomain.BaseDirectory;
            //var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);

            // No Docker se não tiver permissão de escrita no diretório usar sempre FileMode.Open, FileAccess.Read, FileShare.Read
            // senão receberá erro de permissão UnauthorizedAccessException 
            // var img = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "img", "logo.png");
            // using var f = new FileStream(img, FileMode.Open, FileAccess.Read, FileShare.Read);
            // var arr = f.GetAllBytes();
            // Logo = ImageHelper.GetImageBase64(arr); 

            Logo = logo;
        }

      
    }
}
