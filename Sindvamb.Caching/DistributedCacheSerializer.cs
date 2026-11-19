#region

using System.Text;
using Newtonsoft.Json;
using Sindvamb.Utils.Extension;
using Sindvamb.Utils.Services.DependencyInjection;

#endregion

namespace Sindvamb.Caching;

public class DistributedCacheSerializer : IDistributedCacheSerializer, ITransientDependency
{
    public byte[] Serialize<T>(T obj)
    {
        var value = Encoding.UTF8.GetBytes(obj.Serialize());
        return value;
    }

    public T Deserialize<T>(byte[] bytes)
    {
        var value = JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytes));
        return value;
    }
}
