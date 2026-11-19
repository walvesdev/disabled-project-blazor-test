using Sindvamb.Web.Shared.Utils.CustomExceptions;
namespace Sindvamb.Web.Shared.Utils.CustomExceptions;

/// <summary>
///     This class will allow to generate the custom exception message.
/// </summary>
public class CustomException : Exception
{
    public ErrorLogModel ErrorLogModel { get; set; }

    public override string Message { get;  }


    public CustomException()
    {

    }

    public CustomException(string message) : base(message)
    {
        Message = message;
    }

    

    public CustomException(ErrorLogModel errors)
    {
        ErrorLogModel = errors;
    }

   

}
