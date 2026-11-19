using System.Text;

namespace Sindvamb.Web.Shared.Utils
{
    public static class Codificacao
    {
        public static string ToStringFromBase64(this string texto)
        {
            byte[] binario = Convert.FromBase64String(texto);
            return Encoding.ASCII.GetString(binario);
        }

        public static string ToBase64String(this string texto)
        {
            byte[] binario = Encoding.ASCII.GetBytes(texto);
            return Convert.ToBase64String(binario);
        }
    }
}
