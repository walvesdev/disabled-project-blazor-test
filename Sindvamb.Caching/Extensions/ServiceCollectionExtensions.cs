#region

#endregion

using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Sindvamb.Utils;
using StackExchange.Redis;

namespace Sindvamb.Caching.Extensions;

public static class ServiceCollectionExtensions
{

    public static IServiceCollection AddDataProtectionAndRedis(this IServiceCollection services,
        AppSettingsSection appSettings)
    {
        if (appSettings.Redis.UseRedis)
        {
            var conn = AppSettingsSection.RedisConnectionSection.ConnectionType.Default;

            var redisConnectionString = appSettings.Redis.GetConnectionString(conn);
            var redis = ConnectionMultiplexer.Connect(redisConnectionString);

            services.AddDataProtection()
                .SetApplicationName("Sindvamb")
                .PersistKeysToStackExchangeRedis(redis, "Sindvamb-DataProtection-Keys");

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "Sindvamb";
            });
        }

        return services;
    }
}
