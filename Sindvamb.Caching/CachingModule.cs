#region

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sindvamb.Caching.Extensions;
using Sindvamb.Caching.StackExchangeRedis;
using Sindvamb.Utils;

#endregion

namespace Sindvamb.Caching;

public static class CachingModule
{
    public static void AddCachingModule(this IServiceCollection services, IConfiguration configuration,
        AppSettingsSection appSettings)
    {
        services.AddMemoryCache();
        services.AddDistributedMemoryCache();
        services.AddCachingStackExchangeRedis(configuration);
        services.AddDataProtectionAndRedis(appSettings);

        services.AddSingleton(typeof(IDistributedCache<>), typeof(DistributedCache<>));
        services.AddSingleton(typeof(IDistributedCache<,>), typeof(DistributedCache<,>));

        services.Configure<DistributedCacheOptions>(cacheOptions => { cacheOptions.GlobalCacheEntryOptions.SlidingExpiration = TimeSpan.FromMinutes(20); });
    }
}
