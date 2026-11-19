namespace Sindvamb.Events.BuildingBlocks.CommonMessages.DomainEvents;

/// <summary>
///     É um tipo especifica de evento que serve pra notificar um contexto delimitado (handler) pra falar que alguma coisa
///     aconteceu,
///     que houve uma mudança de estado no servidor;
/// </summary>
public class DomainEvent : Message, IDomainEvent
{
    public DomainEvent(dynamic aggregateId)
    {
        AggregateId = aggregateId;
        Timestamp = DateTime.UtcNow;
    }

    public DateTime Timestamp { get; }
}
