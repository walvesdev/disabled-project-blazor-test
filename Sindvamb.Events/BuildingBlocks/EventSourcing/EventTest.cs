#region

#endregion

namespace Sindvamb.Events.BuildingBlocks.EventSourcing;

public class EventTest : Event
{
    public EventTest(Guid id, string data)
    {
        Id = id;
        AggregateId = id;
        Data = data;
    }

    public Guid Id { get; }

    public string Data { get; set; }
}

public class EventTestHandler : INotificationHandler<EventTest>
{
    public IMediatorHandler MediatorHandler { get; set; }

    public EventTestHandler(IMediatorHandler mediatorHandler)
    {
        MediatorHandler = mediatorHandler;
    }

    public async Task Handle(EventTest notification, CancellationToken cancellationToken)
    {
        await MediatorHandler.PublishEvent(notification);
        Console.WriteLine(notification.ToString());
    }
}
