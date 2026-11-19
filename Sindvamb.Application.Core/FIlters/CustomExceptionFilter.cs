#region

using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using Sindvamb.Application.Core.FIlters;
using Sindvamb.Utils.Extension;
using Sindvamb.Utils.Filters.CustomExceptions;

#endregion

namespace Sindvamb.Application.Core.FIlters;

/// <summary>
///     Essa classe é usada para manipular a exceção personalizada no nível do aplicativo.
/// </summary>
public class CustomExceptionFilter : IExceptionFilter
{
    private readonly ILogger<CustomExceptionFilter> Logger;

    public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
    {
        Logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception ?? null;
        var hasException = exception != null;
        var hasInnerException = exception?.InnerException != null;

        string ErrorMessage = null;
        string StackTrace = null;
        // string FullStackTrace = null;
        string InnerStackTrace = null;
        Exception InnerException = null;
        string InnerExceptionMessage = null;
        string Model = null;
        string UserMessage = null;
        object Data = null;
        var CreateLog = true;
        ErrorLogModel Errors;
        int StatusCode = 422;

        var response = context.HttpContext.Response;
        response.ContentType = "application/json";

        if (hasException)
        {
            if (!exception.Message.Contains("inner exception"))
            {
                ErrorMessage = exception.Message;
            }

            if (hasInnerException)
            {
                InnerStackTrace = new StackTrace(exception.InnerException, true).ToString();
            }

            if (exception.InnerException != null)
            {
                InnerException = exception.InnerException;

                if (!exception.InnerException.Message.IsNullOrWhiteSpace())
                {
                    InnerExceptionMessage = exception.InnerException.Message;
                }
            }

            //FullStackTrace = exception.StackTrace;
            StackTrace = new StackTrace(exception, true).ToString();
        }

        switch (exception)
        {
            case CustomException customException:

                var hasCustomException = customException.ValidationResult?.Exception != null;
                var hasCustomInnerException = customException.ValidationResult?.InnerException != null;
                var hasResponseModel = customException.ValidationResult != null;

                if (hasResponseModel && hasCustomException)
                {
                    //FullStackTrace = customException.ValidationResult?.Exception.StackTrace;
                    StackTrace = new StackTrace(customException.ValidationResult?.Exception, true)?.ToString();
                }

                if (hasResponseModel && hasCustomInnerException)
                {
                    InnerStackTrace = new StackTrace(customException.ValidationResult?.InnerException, true)?.ToString();
                }

                if (hasResponseModel)
                {
                    var ResponseModel = customException.ValidationResult;


                    if ((int)customException.ValidationResult.StatusCode > 0)
                    {
                        response.StatusCode = (int)customException.ValidationResult.StatusCode;
                        StatusCode = (int)customException.ValidationResult.StatusCode;
                    }

                    if (!ResponseModel.ModelName.IsNullOrWhiteSpace())
                    {
                        Model = ResponseModel.ModelName;
                    }

                    if (!ResponseModel.ExceptionMessage.IsNullOrWhiteSpace() &&
                        !ResponseModel.ExceptionMessage.Contains("inner exception"))
                    {
                        ErrorMessage = ResponseModel.ExceptionMessage;
                    }

                    if (!ResponseModel.UserMessage.IsNullOrWhiteSpace())
                    {
                        UserMessage = ResponseModel.UserMessage;
                    }

                    if (ResponseModel.Data != null)
                    {
                        Data = ResponseModel.Data;
                    }

                    CreateLog = customException.ValidationResult?.CreateLog ?? false;
                }

                if (customException.InnerException != null)
                {
                    InnerException = customException.InnerException;

                    if (!customException.InnerException.Message.IsNullOrWhiteSpace())
                    {
                        InnerExceptionMessage = customException.InnerException.Message;
                    }
                }

                Errors = exception.As<CustomException>().ErrorLogModel;

                break;
        }




        #region Logging

        if (CreateLog)
        {
            Logger.LogError(new
            {
                UserMessage,
                ExceptionMessage = ErrorMessage,
                InnerExceptionMessage,
                exception.Source,
                StackTrace,
                StatusCode,
                InnerStackTrace,
                InnerException = InnerException?.Serialize(),
                Errors = exception.Is<CustomException>() ? exception.As<CustomException>()?.Errors.Serialize() : null,
                Data
            }.Serialize());
        }

        #endregion Logging

        var Result = new CustomExceptionFilterResult();

        try
        {
            if (exception.Is<CustomException>())
            {
                Result = new CustomExceptionFilterResult
                {
                    Message = (string)(!UserMessage.IsNullOrWhiteSpace() ? JsonConvert.DeserializeObject(UserMessage) : null),
                    SistemMessage = (string)(UserMessage.IsNullOrWhiteSpace() && !ErrorMessage.IsNullOrWhiteSpace()
                        ? JsonConvert.DeserializeObject(ErrorMessage)
                        : null),
                    InnerMessage = UserMessage.IsNullOrWhiteSpace() && !InnerExceptionMessage.IsNullOrWhiteSpace()
                        ? InnerExceptionMessage
                        : null,
                    Errors = exception.As<CustomException>()?.Errors,
                    Data = Data
                };
            }
            else
            {
                Result = new CustomExceptionFilterResult
                {
                    Message = (string)(!UserMessage.IsNullOrWhiteSpace() ? JsonConvert.DeserializeObject(UserMessage) : null),
                    SistemMessage = (string)(UserMessage.IsNullOrWhiteSpace() && !ErrorMessage.IsNullOrWhiteSpace()
                        ? JsonConvert.DeserializeObject(ErrorMessage)
                        : null),
                    InnerMessage = UserMessage.IsNullOrWhiteSpace() && !InnerExceptionMessage.IsNullOrWhiteSpace()
                        ? InnerExceptionMessage
                        : null,
                    Data = Data
                };
            }
        }
        catch (JsonReaderException)
        {
            try
            {
                if (exception.Is<CustomException>())
                {
                    Result = new CustomExceptionFilterResult
                    {
                        Message = UserMessage,
                        SistemMessage = (string)(UserMessage.IsNullOrWhiteSpace() && !ErrorMessage.IsNullOrWhiteSpace()
                            ? JsonConvert.DeserializeObject(ErrorMessage)
                            : null),
                        InnerMessage = UserMessage.IsNullOrWhiteSpace() && !InnerExceptionMessage.IsNullOrWhiteSpace()
                            ? InnerExceptionMessage
                            : null,
                        Errors = exception.As<CustomException>()?.Errors,
                        Data = Data
                    };
                }
                else
                {
                    Result = new CustomExceptionFilterResult
                    {
                        Message = UserMessage,
                        SistemMessage = (string)(UserMessage.IsNullOrWhiteSpace() && !ErrorMessage.IsNullOrWhiteSpace()
                            ? JsonConvert.DeserializeObject(ErrorMessage)
                            : null),
                        InnerMessage = UserMessage.IsNullOrWhiteSpace() && !InnerExceptionMessage.IsNullOrWhiteSpace()
                            ? InnerExceptionMessage
                            : null,
                        Data = Data
                    };
                }
            }
            catch (JsonReaderException)
            {
                try
                {
                    if (exception.Is<CustomException>())
                    {
                        Result = new CustomExceptionFilterResult
                        {
                            Message = (string)(!UserMessage.IsNullOrWhiteSpace() ? JsonConvert.DeserializeObject(UserMessage) : null),
                            SistemMessage = (string)(UserMessage.IsNullOrWhiteSpace() && !ErrorMessage.IsNullOrWhiteSpace()
                                ? JsonConvert.DeserializeObject(ErrorMessage)
                                : null),
                            InnerMessage = UserMessage.IsNullOrWhiteSpace() && !InnerExceptionMessage.IsNullOrWhiteSpace()
                                ? InnerExceptionMessage
                                : null,
                            Errors = exception.As<CustomException>()?.Errors,
                            Data = Data
                        };
                    }
                    else
                    {
                        Result = new CustomExceptionFilterResult
                        {
                            Message = (string)(!UserMessage.IsNullOrWhiteSpace() ? JsonConvert.DeserializeObject(UserMessage) : null),
                            SistemMessage = (string)(UserMessage.IsNullOrWhiteSpace() && !ErrorMessage.IsNullOrWhiteSpace()
                                ? JsonConvert.DeserializeObject(ErrorMessage)
                                : null),
                            InnerMessage = UserMessage.IsNullOrWhiteSpace() && !InnerExceptionMessage.IsNullOrWhiteSpace()
                                ? InnerExceptionMessage
                                : null,
                            Data = Data
                        };
                    }
                }
                catch (JsonReaderException)
                {
                    if (exception.Is<CustomException>())
                    {
                        Result = new CustomExceptionFilterResult
                        {
                            Message = UserMessage,
                            SistemMessage = UserMessage.IsNullOrWhiteSpace() && !ErrorMessage.IsNullOrWhiteSpace()
                                ? ErrorMessage
                                : null,
                            InnerMessage = UserMessage.IsNullOrWhiteSpace() && !InnerExceptionMessage.IsNullOrWhiteSpace()
                                ? InnerExceptionMessage
                                : null,
                            Errors = exception.As<CustomException>()?.Errors,
                            Data = Data
                        };
                    }
                    else
                    {
                        Result = new CustomExceptionFilterResult
                        {
                            Message = UserMessage,
                            SistemMessage = UserMessage.IsNullOrWhiteSpace() && !ErrorMessage.IsNullOrWhiteSpace()
                                ? ErrorMessage
                                : null,
                            InnerMessage = UserMessage.IsNullOrWhiteSpace() && !InnerExceptionMessage.IsNullOrWhiteSpace()
                                ? InnerExceptionMessage
                                : null,
                            Data = Data
                        };
                    }
                }
            }
        }

        //context.Result = new JsonResult(Result, new JsonSerializerSettings
        //{
        //    Formatting = Formatting.Indented,
        //    NullValueHandling = NullValueHandling.Ignore
        //});

        // context.Result = new UnprocessableEntityObjectResult(new JsonResult(Result, new JsonSerializerSettings
        // {
        //     Formatting = Formatting.Indented,
        //     PreserveReferencesHandling = PreserveReferencesHandling.All,
        //     NullValueHandling = NullValueHandling.Ignore
        // })?.Value);


        var result = new ContentResult() { Content = Result?.Serialize(), StatusCode = StatusCode };

        context.Result = result;

        context.ExceptionHandled = true;
    }
}
