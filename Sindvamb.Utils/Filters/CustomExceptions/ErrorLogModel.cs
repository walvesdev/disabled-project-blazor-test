using Sindvamb.Utils;

namespace Sindvamb.Utils.Filters.CustomExceptions;

public class ErrorLogModel
{
    public string UserMessage { get; set; }
    public string ExceptionMessage { get; set; }
    public string ExceptionStackTrace { get; set; }
    public string InnerExceptionMessage { get; set; }
    public string InnerExceptionStackTrace { get; set; }
    public string Line { get; set; }
    public string ClassName { get; set; }
    public List<ValidationExceptionResult> Errors { get; set; } = new();
    public object Data { get; set; }
}
