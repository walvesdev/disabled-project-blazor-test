#region

using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading;

#endregion

namespace Sindvamb.Utils.Utils;

public class ConventUtil
{
    public static string ObjectToJSON2<T>(T obj)
    {
        var serializer = new DataContractJsonSerializer(typeof(T));
        var result = string.Empty;
        using (var ms = new MemoryStream())
        {
            serializer.WriteObject(ms, obj);
            ms.Position = 0;

            using (var read = new StreamReader(ms))
            {
                result = read.ReadToEnd();
            }
        }

        return result;
    }


    public static T JsonToObject2<T>(string jsonText)
    {
        var s = new DataContractJsonSerializer(typeof(T));
        var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonText));
        var obj = (T)s.ReadObject(ms);
        ms.Dispose();
        return obj;
    }

    public static void TaskMethod2(object dd, bool ThreadInfo = true)
    {
        Console.WriteLine(dd);
        if (ThreadInfo == false) return;

        Console.WriteLine("Task id :{0},Thread :{1}", Task.CurrentId, Thread.CurrentThread.ManagedThreadId);
        Console.WriteLine("Is pool thread :{0}", Thread.CurrentThread.IsThreadPoolThread);
        Console.WriteLine("Is background thread:{0}", Thread.CurrentThread.IsBackground);
        Console.WriteLine("");
    }

    public static string ObjectToJson(object obj) => JsonConvert.SerializeObject(obj);

    public static object JsonToObject(string jsonString, object obj) => JsonConvert.DeserializeObject(jsonString, obj.GetType());

    public static IEnumerable<string> GetEnumDescriptions(Enum value)
    {
        var descs = new List<string>();
        var type = value.GetType();
        var name = Enum.GetName(type, value);
        var field = type.GetField(name);
        var fds = field.GetCustomAttributes(typeof(DescriptionAttribute), true);
        foreach (DescriptionAttribute fd in fds) descs.Add(fd.Description);
        return descs;
    }

    public static bool IsNumeric(object anyObject)
    {
        try
        {
            if (anyObject == null || anyObject is DBNull) return false;

            var anyString = anyObject.ToString();
            if (anyString.Length > 0)
            {
                var dummyOut = new double();
                var cultureInfo =
                    new CultureInfo("en-US", true);

                return double.TryParse(anyString, NumberStyles.Any,
                    cultureInfo.NumberFormat, out dummyOut);
            }

            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static string passwordhash(string password)
    {
        string result;
        using (var cryptoProvider = SHA1.Create())
        {
            var passwordHash = cryptoProvider.ComputeHash(Encoding.UTF8.GetBytes(password));
            result = "new byte[] { " +
                     string.Join(",", passwordHash.Select(x => "0x" + x.ToString("x2")).ToArray())
                     + " } ";
        }

        return result;
    }


    public static DataTable GenericToDataTable<T>(IList<T> list)
    {
        var json = JsonConvert.SerializeObject(list);
        var dt = (DataTable)JsonConvert.DeserializeObject(json, typeof(DataTable));
        return dt;
    }


    public static IList<T> DataTableToGeneric<T>(DataTable dt)
    {
        var json = JsonConvert.SerializeObject(dt);
        var list = JsonConvert.DeserializeObject<IList<T>>(json);
        return list;
    }


    public static Stream DataUrl2Stream(string base64encodedstring)
    {
        var base64Data = Regex.Match(base64encodedstring, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
        var bytes = Convert.FromBase64String(base64Data);
        var stream = new MemoryStream(bytes);
        return stream;
    }


    public static Stream Base642Stream(string base64encodedstring)
    {
        var bytes = Convert.FromBase64String(base64encodedstring);
        var stream = new MemoryStream(bytes);
        return stream;
    }

    public string ImageFromDataUri(Uri src)
    {
        using (var wc = new System.Net.WebClient())
        {
            var imageBytes = wc.DownloadData(src);
            var imageBase64 = Convert.ToBase64String(imageBytes);
            var mimeType = wc.ResponseHeaders["content-type"];
            if (mimeType.Contains(';'))
                mimeType = mimeType.Split(';')[0];
            return $"data:{mimeType};base64,{imageBase64}";
        }
    }
}
