#region

using EventStore.ClientAPI;

#endregion

namespace Sindvamb.Events.BuildingBlocks.EventSourcing;

public interface IEventStoreService
{
    IEventStoreConnection GetConnection();
}
