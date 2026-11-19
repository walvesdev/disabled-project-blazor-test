#region

using Microsoft.AspNetCore.Mvc.ModelBinding;

#endregion

namespace Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;

public class DomainNotificationHandler : INotificationHandler<DomainNotification>
{
    public List<DomainNotification> Notifications { get; set; }

    public DomainNotificationHandler()
    {
        Notifications = new List<DomainNotification>();
    }

    public Task Handle(DomainNotification message, CancellationToken cancellationToken)
    {
        Notifications.Add(message);
        return Task.CompletedTask;
    }

    public virtual void AddNotification(DomainNotification notification) => Notifications.Add(notification);

    public virtual List<DomainNotification> GetNotifications()
    {
        return Notifications;
    }

    public virtual bool HasNotification()
    {
        return GetNotifications().Any();
    }

    public virtual bool HasNotification(ModelStateDictionary modelState = null)
    {
        var hasNotification = HasNotification();

        if (hasNotification && modelState is not null) Notifications.ForEach(c => modelState.AddModelError(string.Empty, c.Validator));

        return hasNotification;
    }

    public void Dispose() => Notifications = new List<DomainNotification>();


}
