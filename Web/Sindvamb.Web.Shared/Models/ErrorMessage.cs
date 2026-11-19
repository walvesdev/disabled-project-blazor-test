using Sindvamb.Web.Shared.Models;

namespace Sindvamb.Web.Shared.Models;

public class ErrorMessage
{
    public ValueMessage Value { get; set; }
}

public class ValidationError
{
    public string Key { get; set; }
    public string Validator { get; set; }
    public DateTime Timestamp { get; set; }
    public string DomainNotificationId { get; set; }
    public int Version { get; set; }
    public string AggregateId { get; set; }
    public string MessageType { get; set; }
}

public class ValueMessage
{
    public int Code { get; set; }
    public string Message { get; set; }
    public List<ValidationError> ValidationErrors { get; set; }
}
