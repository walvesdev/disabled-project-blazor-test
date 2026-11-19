#region

using EventStore.ClientAPI;

using Microsoft.Extensions.Configuration;

#endregion

namespace Sindvamb.Events.BuildingBlocks.EventSourcing;

public class EventStoreService : IEventStoreService
{
    private readonly IEventStoreConnection _connection;

    public EventStoreService(IConfiguration configuration)
    {
        UseEventStore = configuration.GetValue<bool>("AppSettings:EventStore:Enabled");

        if (!UseEventStore) return;

        _connection = EventStoreConnection.Create(
            configuration.GetConnectionString("EventStoreConnection"));

        _connection.ConnectAsync();
    }

    public bool UseEventStore { get; set; }

    public IEventStoreConnection GetConnection() => UseEventStore ? _connection : default!;
}
