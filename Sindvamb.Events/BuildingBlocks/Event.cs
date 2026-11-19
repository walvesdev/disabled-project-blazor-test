namespace Sindvamb.Events.BuildingBlocks;

public abstract class Event : Message, IEvent
{
    protected Event()
    {
        Timestamp = DateTime.UtcNow;
    }

    public DateTime Timestamp { get; }
}
