
using Microsoft.Extensions.Caching.Distributed;

namespace Sindvamb.Caching;

public class DistributedCacheOptions
{
    /// <summary>
    ///     Lance ou oculte exceções para o cache distribuído.
    /// </summary>
    public bool HideErrors { get; set; } = true;

    /// <summary>
    ///     Prefixo da chave de cache.
    /// </summary>
    public string KeyPrefix { get; set; }

    /// <summary>
    ///     Opções de entrada do cache global.
    /// </summary>
    public DistributedCacheEntryOptions GlobalCacheEntryOptions { get; set; }

    /// <summary>
    ///     Lista de todos os configuradores de cache.
    ///     (func argument:Name of cache)
    /// </summary>
    public List<Func<string, DistributedCacheEntryOptions>> CacheConfigurators { get; set; } //TODO: use a configurator interface instead?

    public DistributedCacheOptions()
    {
        CacheConfigurators = new List<Func<string, DistributedCacheEntryOptions>>();
        GlobalCacheEntryOptions = new DistributedCacheEntryOptions();
        KeyPrefix = "";
    }

    public void ConfigureCache<TCacheItem>(DistributedCacheEntryOptions options)
    {
        ConfigureCache(typeof(TCacheItem), options);
    }

    public void ConfigureCache(Type cacheItemType, DistributedCacheEntryOptions options)
    {
        ConfigureCache(CacheNameAttribute.GetCacheName(cacheItemType), options);
    }

    public void ConfigureCache(string cacheName, DistributedCacheEntryOptions options)
    {
        CacheConfigurators.Add(name => cacheName != name ? null : options);
    }
}
