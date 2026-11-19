#region

#endregion

using Sindvamb.Utils;
#region

#endregion

using Sindvamb.Utils.Extension;
#region

#endregion

using Sindvamb.Utils.Filters.CustomExceptions;

#region

#endregion

namespace Sindvamb.Utils.Services.Http.Models;

public class HttpClientResult<T>
{
    private string? _errorMessage;

    private Exception _exception;

    public bool IsSuccessStatus { get; set; }

    public T Data { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public HttpResponseMessage HttpResponseMessage { get; set; }

    public List<ValidationExceptionResult> Errors { get; set; } = new();

    public string? ErrorMessage
    {
        get
        {
            if (_errorMessage.IsNullOrEmpty() && Exception.IsNull()) return Enum.GetName(typeof(HttpStatusCode), StatusCode);

            return _errorMessage;
        }
        set
        {
            _errorMessage = value;
            //if (!string.IsNullOrWhiteSpace(_errorMessage) && Exception == null) AppServices.SetLogErrors<HttpClientResult<T>>(message: _errorMessage.RemoveScape());
        }
    }

    public Exception Exception
    {
        get => _exception;
        set
        {
            _exception = value;
            //if (_errorMessage != null)
            //    AppServices.SetLogErrors<HttpClientResult<T>>(_exception, StatusCode,
            //        _errorMessage.RemoveScape());
        }
    }

    public ErrorLogModel ErrorLogModel { get; set; }

    public void Validate()
    {
        if (!IsSuccessStatus)
        {
            if (ErrorLogModel.IsNotNull())
                throw new CustomException(ErrorLogModel);
            if (!Errors.IsNullOrEmpty())
                throw new CustomException(Errors);
            if (!ErrorMessage.IsNullOrWhiteSpace())
                throw new CustomException(ErrorMessage);
            throw new CustomException("Aconteceu algo inesperado, por favor contate o suporte!");
        }
    }

    //public async Task CheckSuccessStatus(SwalMessageService SwalMessageService)
    //{
    //    if (!IsSuccessStatus)
    //    {

    //        var op = new SwalOption()
    //        {
    //            Category = SwalCategory.Error,
    //            Title = "Oops...",
    //            ShowFooter = false,
    //            IsAutoHide = false,
    //            Delay = 4000,
    //            //FooterTemplate = BootstrapDynamicComponent.CreateComponent<SwalFooter>().Render()
    //        };

    //        var errorMessage = new StringBuilder();

    //        if (ErrorLogModel.IsNotNull())
    //        {
    //            errorMessage.AppendLine(ErrorLogModel.UserMessage);
    //            errorMessage.AppendLine(ErrorLogModel.ExceptionMessage);
    //            errorMessage.AppendLine(ErrorLogModel.InnerExceptionMessage);

    //            if (!ErrorLogModel.Errors.IsNullOrEmpty())
    //            {
    //                ErrorLogModel.Errors.ForEach(erro =>
    //                {
    //                    errorMessage.AppendLine(erro.Message);
    //                    errorMessage.AppendLine(erro.UserMessage);
    //                    errorMessage.AppendLine(erro.ExceptionMessage);
    //                    errorMessage.AppendLine(erro.InnerExceptionMessage);
    //                });
    //            }
    //            op.Content = errorMessage.ToString();
    //            await SwalMessageService.Show(op);
    //        }
    //        else if (!Errors.IsNullOrEmpty())
    //        {
    //            Errors.ForEach(erro =>
    //            {
    //                errorMessage.AppendLine(erro.Message);
    //                errorMessage.AppendLine(erro.UserMessage);
    //                errorMessage.AppendLine(erro.ExceptionMessage);
    //                errorMessage.AppendLine(erro.InnerExceptionMessage);
    //            });

    //            op.Content = errorMessage.ToString();
    //            await SwalMessageService.Show(op);
    //        }
    //        else if (!ErrorMessage.IsNullOrWhiteSpace())
    //        {
    //            op.Content = ErrorMessage;
    //            await SwalMessageService.Show(op);
    //        }
    //        else
    //        {
    //            op.Content = "Aconteceu algo inesperado, por favor contate o suporte!";
    //            await SwalMessageService.Show(op);
    //        }
    //    }
    //}
}
