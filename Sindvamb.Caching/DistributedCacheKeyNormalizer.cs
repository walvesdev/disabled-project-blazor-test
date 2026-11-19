#region

#endregion

using Microsoft.Extensions.Options;
using Sindvamb.Utils.Services.DependencyInjection;

namespace Sindvamb.Caching;

public class DistributedCacheKeyNormalizer : IDistributedCacheKeyNormalizer, ITransientDependency
{
    protected DistributedCacheOptions DistributedCacheOptions { get; }

    public DistributedCacheKeyNormalizer(
        IOptions<DistributedCacheOptions> distributedCacheOptions)
    {
        DistributedCacheOptions = distributedCacheOptions.Value;
    }

    public virtual string NormalizeKey(DistributedCacheKeyNormalizeArgs args)
    {
        // var normalizedKey = $"c:{args.CacheName},k:{DistributedCacheOptions.KeyPrefix}{args.Key}";
        var normalizedKey = $":{args.CacheName}:{DistributedCacheOptions.KeyPrefix}{args.Key}";

        return normalizedKey;
    }
}
