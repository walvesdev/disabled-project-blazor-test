namespace Sindvamb.Web.Shared.Utils
{
    public static class FileConverter
    {
        public static async Task<string> ConvertFileToStringAsync(Stream fileStream)
        {
            var converter = new StreamReader(fileStream);
            return await converter.ReadToEndAsync();
        }
    }
}
