using Sindvamb.Web.Shared.Helpers;

namespace Sindvamb.Web.Shared.Helpers;

public class ImageHelper
{
    public static string GetImageBase64(byte[] arr)
    {
        using Stream fs = new MemoryStream(arr);
        BinaryReader br = new BinaryReader(fs);

        var bytes = br.ReadBytes((Int32)fs.Length);
        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
        var imageUrl = "data:image/png;base64," + base64String;

        return imageUrl;
    }
}
