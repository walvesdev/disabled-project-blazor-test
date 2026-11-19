using Sindvamb.Domain.Entities;
using Sindvamb.HangFire.Host.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace Sindvamb.HangFire.Host.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty] public Usuario? Usuario { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var content = new StringContent(JsonConvert.SerializeObject(Usuario), Encoding.UTF8,
                HttpClientMediaType.ApplicationJson);

            var handler = new HttpClientHandler
            {
                AutomaticDecompression =
                    DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.All
            };
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            var client = new HttpClient(handler);
            var response = await client.PostAsync(AppServices.GetSection<string>("AppSettings:ApiLoginUrl"), content);

            var result = await response.Content.ReadFromJsonAsync<JwtAccessToken>();

            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(30)
            };
            Response.Cookies.Append(AppServices.GetSection<string>("AppSettings:CookieName"), result.Token, cookieOptions);


            return Redirect($"{AppServices.GetSection<string>("AppSettings:BaseUrl")}/home");
        }
    }

    public class Usuario
    {
        public int Matricula { get; set; }
        public string Password { get; set; }
    }
}