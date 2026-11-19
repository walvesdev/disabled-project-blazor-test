namespace Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;

public interface IDomainNotification : IMessage
{
    public Guid DomainNotificationId { get; set; }
    public string Key { get; set; }
    public string Validator { get; set; }
    public int Version { get; set; }
    public DateTime Timestamp { get; }
}
