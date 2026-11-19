using Sindvamb.Utils;
using Sindvamb.Utils.Extension;
using Sindvamb.Utils.Filters.CustomExceptions;

namespace Sindvamb.Utils.Filters.CustomExceptions;

/// <summary>
///     This class will allow to generate the custom exception message.
/// </summary>
public class CustomException : Exception
{
    public CustomException()
    {
        ValidationResult = new ValidationExceptionResult();
    }

    public CustomException(string message) : base(message)
    {
        ValidationResult = new ValidationExceptionResult
        {
            UserMessage = message
        };
    }

    public CustomException(ValidationExceptionResult validationError) : base(
        validationError.UserMessage ?? validationError.ExceptionMessage, validationError.InnerException)
    {
        ValidationResult = validationError;
    }

    public CustomException(string userMessage, string modelName, Exception exception, bool createLog = true) : base(
        exception.Message, exception.InnerException)
    {
        ValidationResult = new ValidationExceptionResult
        {
            Exception = exception,
            ExceptionMessage = exception.Message,
            InnerException = exception.InnerException,
            InnerExceptionMessage = exception.InnerException?.Message,
            ModelName = modelName,
            UserMessage = userMessage,
            CreateLog = createLog
        };

        if (exception.IsNull() && userMessage.IsNullOrEmpty()) ValidationResult.UserMessage = "Falha ao procesar requisição!";
    }

    public CustomException(Exception? exception, bool createLog = true, string? userMessage = null) : base(
        exception?.Message, exception?.InnerException)
    {
        ValidationResult = new ValidationExceptionResult();

        if (exception != null)
        {
            ValidationResult.Exception = exception;
            ValidationResult.ExceptionMessage = exception.Message;
            ValidationResult.InnerException = exception.InnerException;
            ValidationResult.InnerExceptionMessage = exception.InnerException?.Message;
        }

        ValidationResult.UserMessage = userMessage;
        ValidationResult.CreateLog = createLog;
        if (exception.IsNull() && userMessage.IsNullOrEmpty()) ValidationResult.UserMessage = "Falha ao procesar requisição!";
    }

    public CustomException(Exception? exception, string? userMessage = null) : base(exception?.Message,
        exception?.InnerException)
    {
        ValidationResult = new ValidationExceptionResult();

        if (exception != null)
        {
            ValidationResult.Exception = exception;
            ValidationResult.ExceptionMessage = exception.Message;
            ValidationResult.InnerException = exception.InnerException;
            ValidationResult.InnerExceptionMessage = exception.InnerException?.Message;
        }

        ValidationResult.UserMessage = userMessage;
        ValidationResult.CreateLog = true;
        if (exception.IsNull() && userMessage.IsNullOrEmpty()) ValidationResult.UserMessage = "Falha ao procesar requisição!";
    }

    public CustomException(List<ValidationExceptionResult> errors)
    {
        Errors = errors;
    }

    public CustomException(ErrorLogModel errors)
    {
        ErrorLogModel = errors;
    }

    public List<ValidationExceptionResult> Errors { get; set; }
    public ErrorLogModel ErrorLogModel { get; set; }

    public ValidationExceptionResult ValidationResult { get; set; }
}
