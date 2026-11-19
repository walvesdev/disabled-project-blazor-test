using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Sindvamb.Templates.Pages
{
    public class BasePage : PageModel
    {
        public string StaticFileServerUrl { get; set; }
        public string CssClassName { get; set; }
    }
}
