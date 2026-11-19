#region

#endregion

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;

namespace Sindvamb.Caching.StackExchangeRedis;

public static class CachingStackExchangeRedis
{
    public static void AddCachingStackExchangeRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var appSettings = AppServices.GetAppSettingsSection();

        var redisConnectionString = appSettings.Redis.GetConnectionString(AppSettingsSection.RedisConnectionSection.ConnectionType.Default);

        if (appSettings.Redis.UseRedis)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                if (!redisConnectionString.IsNullOrEmpty())
                {
                    options.Configuration = redisConnectionString;
                }
            });

            services.Replace(ServiceDescriptor.Singleton<IDistributedCache, RedisCache>());
        }
    }
}
