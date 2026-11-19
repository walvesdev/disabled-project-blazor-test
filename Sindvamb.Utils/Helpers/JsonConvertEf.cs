#region

#endregion

namespace Sindvamb.Utils.Helpers;

public static class JsonConvertEf
{
    public static string Sereialize(object o)
    {
        return JsonConvert.SerializeObject(o);
    }

    public static T Deserialize<T>(string str)
    {
        return JsonConvert.DeserializeObject<T>(str);
    }
}
