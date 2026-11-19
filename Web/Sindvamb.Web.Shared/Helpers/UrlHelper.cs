namespace Sindvamb.Web.Shared.Helpers
{
    public static class UrlHelper
    {
        public static string EditUrl { get; set; }

        public static string ImageFromDataUri(Uri src)
        {
            using var wc = new System.Net.WebClient();
            var imageBytes = wc.DownloadData(src);
            var imageBase64 = Convert.ToBase64String(imageBytes);
            var mimeType = wc.ResponseHeaders["content-type"];
            if (mimeType.Contains(';'))
                mimeType = mimeType.Split(';')[0];
            return $"data:{mimeType};base64,{imageBase64}";
        }
    }
}
