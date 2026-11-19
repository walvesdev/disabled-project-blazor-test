#region

#endregion

namespace Sindvamb.Utils;

[Serializable]
public class ValidationExceptionResult
{
    public ValidationExceptionResult()
    {
    }

    public ValidationExceptionResult(string message)
    {
        Message = message;
    }

    public string Message { get; set; }

    public string? UserMessage { get; set; }

    public string ExceptionMessage { get; set; }

    public HttpStatusCode? StatusCode { get; set; } = HttpStatusCode.UnprocessableEntity;

    public string ModelName { get; set; }

    public Exception? Exception { get; set; }

    public Exception? InnerException { get; set; }

    public string? InnerExceptionMessage { get; set; }

    public object Data { get; set; }

    [JsonIgnore] public bool CreateLog { get; internal set; } = true;
}
