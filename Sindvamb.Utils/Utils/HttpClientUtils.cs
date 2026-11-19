#region

using System.IO;

#endregion

namespace Sindvamb.Utils.Utils;

public static class HttpClientUtils
{
    public static List<T> JsonObjects<T>(Uri url)
    {
        using (var client = new HttpClient())
        {
            var result = client.GetStringAsync(url).Result;

            var ds = JsonConvert.DeserializeObject<List<T>>(result);
            return ds;
        }
    }

    public static List<T> JsonObjects<T>(string url) => JsonObjects<T>(new Uri(url));

    public static T JsonObject<T>(Uri url)
    {
        using (var client = new HttpClient())
        {
            var result = client.GetStringAsync(url).Result;

            var ds = JsonConvert.DeserializeObject<T>(result);
            return ds;
        }
    }

    public static T JsonObject<T>(string url) => JsonObject<T>(new Uri(url));

    public static async Task<T> JsonObjectAsync<T>(Uri url)
    {
        using (var client = new HttpClient())
        {
            var result = await client.GetStringAsync(url);

            var ds = JsonConvert.DeserializeObject<T>(result);
            return ds;
        }
    }

    public static async Task<T> JsonObjectAsync<T>(string url) => await JsonObjectAsync<T>(new Uri(url));

    public static async Task DownloadFileTaskAsync(this HttpClient client, Uri uri, string FileName)
    {
        using (var s = await client.GetStreamAsync(uri))
        {
            using (var fs = new FileStream(FileName, FileMode.CreateNew))
            {
                await s.CopyToAsync(fs);
            }
        }
    }

    public static async Task<string> DownloadStringAsync(Uri uri)
    {
        var client = new HttpClient();
        return await client.GetStringAsync(uri);
    }

    public static async Task<string> DownloadStringAsync(string url)
    {
        var client = new HttpClient();
        return await client.GetStringAsync(url);
    }

    public static string DownloadString(string url)
    {
        using (var client = new HttpClient())
        {
            var result = client.GetStringAsync(url).Result;
            return result;
        }
    }
}
