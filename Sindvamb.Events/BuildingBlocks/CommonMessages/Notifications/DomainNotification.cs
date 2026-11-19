#region

#endregion

namespace Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;

/// <summary>
///     As notificações de dominio elas notificam algum problema encontrado atraves da regra de negocio que invalida aquele
///     processo q o usuario esta tenteando faz;
///     Ela traz um lista de problemas encontrado;
/// </summary>
public class DomainNotification : Message, IDomainNotification, INotification
{
    public DomainNotification()
    {
    }

    public DomainNotification(string key, string validator, HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity)
    {

        Timestamp = DateTime.UtcNow.ToFormatedDate();
        DomainNotificationId = Guid.NewGuid();
        Version = 1;
        Key = key;
        Validator = validator;
        StatusCode = statusCode;
    }

    public DomainNotification(string validator, object errors = null, HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity)
    {
        Timestamp = DateTime.UtcNow.ToFormatedDate();
        DomainNotificationId = Guid.NewGuid();
        Version = 1;
        Validator = validator;
        StatusCode = statusCode;
        Errors = errors;
    }

    public Guid DomainNotificationId { get; set; }
    public Guid SessionId { get; set; }
    public string Key { get; set; }
    public string Validator { get; set; }
    public int Version { get; set; }
    public object Errors { get; set; }

    public HttpStatusCode StatusCode { get; set; }
}
