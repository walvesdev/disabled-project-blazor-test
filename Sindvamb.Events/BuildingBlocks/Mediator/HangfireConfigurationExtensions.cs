#region

using Hangfire;

using Newtonsoft.Json;

#endregion

namespace Sindvamb.Events.BuildingBlocks.Mediator;

public static class HangfireConfigurationExtensions
{
    public static void UseMediatR(this IGlobalConfiguration configuration)
    {
        var jsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All
        };
        configuration.UseSerializerSettings(jsonSettings);
    }
}
