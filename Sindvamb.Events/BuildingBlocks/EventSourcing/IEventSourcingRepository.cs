namespace Sindvamb.Events.BuildingBlocks.EventSourcing;

public interface IEventSourcingRepository
{
    Task Save<TEvent>(TEvent domainEvent) where TEvent : IEvent;
    Task<IEnumerable<StoredEvent>> GetEvents(Guid aggregateId);
}
