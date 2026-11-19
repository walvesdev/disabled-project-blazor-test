using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sindvamb.Events.BuildingBlocks.Handlers;

public class GenericNotificationHandler<TNotification> : IGenericNotificationHandler<TNotification>, INotificationHandler<TNotification>
    where TNotification : INotification
{
    public List<KeyValuePair<string, TNotification>> Notifications { get; set; } = new();

    public void AddNotification(TNotification notification)
    {
        if (!Notifications.Any(n => n.Key.Equals(AppServices.GetSessionId())))
        {
            Notifications.Add(new KeyValuePair<string, TNotification>(AppServices.GetSessionId(), notification));
        }
    }

    public List<TNotification> GetNotifications()
    {
        return Notifications.Where(n => n.Key.Equals(AppServices.GetSessionId())).Select(e => e.Value).ToList();
    }

    public bool HasNotification()
    {
        return GetNotifications().Any();
    }

    public bool HasNotification(ModelStateDictionary modelState = null)
    {
        var hasNotification = HasNotification();

        if (hasNotification && modelState is not null) Notifications.ForEach(c => modelState.AddModelError(string.Empty, c.Serialize()));

        return hasNotification;
    }

    public void ClearBySeesion()
    {
        Notifications.RemoveAll(n => n.Key.Equals(AppServices.GetSessionId()));
    }

    public Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        AddNotification(notification);
        return Task.CompletedTask;
    }
}

public interface IGenericNotificationHandler<TNotification> : INotificationHandler<TNotification>
    where TNotification : INotification
{
    List<KeyValuePair<string, TNotification>> Notifications { get; set; }
    void AddNotification(TNotification notification);
    List<TNotification> GetNotifications();
    bool HasNotification();
    bool HasNotification(ModelStateDictionary modelState = null);
}