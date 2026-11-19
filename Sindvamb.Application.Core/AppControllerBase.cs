#region

using System.Net;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Sindvamb.Events.BuildingBlocks.Handlers;
using Sindvamb.Security.Configurations;
using Sindvamb.Utils.Attributes;
using Sindvamb.Application.Core.Extensions;
using Sindvamb.Application.Core.Attributes;
using Sindvamb.Application.Core.Helpers;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Utils;
using Microsoft.AspNetCore.Http;

#endregion

namespace Sindvamb.Application.Core;

[ApiController]
[Route("api/[Controller]/[Action]")]
[ApiSettings]
// [Permission(AuthPolicy.MASTER_ADMIN)]
// [Authorize(Roles = AuthRole.MASTER_ADMIN)]
public abstract class AppControllerBase : ControllerBase, IDisposable
{
    // private readonly DomainNotificationHandler _notifications;
    public GenericNotificationHandler<DomainNotification> Notifications { get; set; }
    public IMediatorHandler MediatorHandler { get; set; }


    protected AppControllerBase(
        INotificationHandler<DomainNotification> notifications,
        IMediatorHandler mediatorHandler)
    {
        // _notifications = (DomainNotificationHandler)notifications;
        Notifications = (GenericNotificationHandler<DomainNotification>)notifications;
        MediatorHandler = mediatorHandler;
    }

    protected bool OperationIsValid()
    {
        if (!Notifications.HasNotification() && !ExceptionNotification.IsException) return true;

        if (ExceptionNotification.IsException)
        {
            ExceptionNotification.IsException = false;
            MediatorHandler.PublishNotification(ExceptionNotification.Notification);
            ExceptionNotification.Notification = null;
        }

        return false;
    }

    protected bool ValidateOperation(ModelStateDictionary modelState)
    {
        return !Notifications.HasNotification(modelState);
    }

    protected IEnumerable<string> GetErrorMessages()
    {
        return Notifications.GetNotifications().Select(c => c.Validator).ToList();
    }

    protected List<DomainNotification> GetNotifications()
    {
        return Notifications.GetNotifications();
    }

    protected void SendError(string code, string message)
    {
        MediatorHandler.PublishNotification(new DomainNotification(code, message));
    }

    protected async Task<IActionResult> Errors()
    {
        var notifications = GetNotifications();

        if (notifications.Count == 1)
        {
            switch (notifications.FirstOrDefault().StatusCode)
            {
                case HttpStatusCode.Forbidden:
                    return Forbid();
                    break;

                case HttpStatusCode.Unauthorized:
                    return Unauthorized(ModelState.ToErrorResponse(notifications));
                    break;

                case HttpStatusCode.BadRequest:
                    return BadRequest(ModelState.ToErrorResponse(notifications));
                    break;
            }
        }

        return await Task.FromResult(UnprocessableEntity(ModelState.ToErrorResponse(notifications)));
    }

    protected UnprocessableEntityObjectResult Error(string message)
    {
        var validationErrors = new List<dynamic>
        {
            new
            {
                message, type = "ValidationResult"
            }
        };

        return UnprocessableEntity(new
        {
            Code = 422,
            Details = "Hove erros de validação!",
            Message = "Erro ao processar requisição!",
            ValidationErrors = validationErrors.ToArray()
        });
    }

    protected async Task<IActionResult> ApiResult<T>(T result = default, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        if (!OperationIsValid())
        {
            return await Errors();
        }


        return await Task.FromResult(GetStatusCodeReturn(result, statusCode));
    }

    protected async Task<IActionResult> ApiResult(HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        if (!OperationIsValid())
        {
            return await Errors();
        }


        return await Task.FromResult(GetStatusCodeReturn("", statusCode: statusCode));
    }

    private IActionResult GetStatusCodeReturn<T>(T result = default, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        if (result == null)
        {
            return statusCode switch
            {
                HttpStatusCode.NotFound => NotFound(),
                HttpStatusCode.Unauthorized => Unauthorized(),
                HttpStatusCode.Forbidden => Forbid(),
                HttpStatusCode.BadRequest => BadRequest(),
                HttpStatusCode.UnprocessableEntity => UnprocessableEntity(),
                HttpStatusCode.NoContent => NoContent(),
                HttpStatusCode.OK => Ok(),
                HttpStatusCode.Created => Created("", null),
                _ => Ok(),
            };
        }

        return statusCode switch
        {
            HttpStatusCode.NotFound => NotFound(result),
            HttpStatusCode.Unauthorized => Unauthorized(),
            HttpStatusCode.Forbidden => Forbid(),
            HttpStatusCode.BadRequest => BadRequest(result),
            HttpStatusCode.UnprocessableEntity => UnprocessableEntity(result),
            HttpStatusCode.NoContent => NoContent(),
            HttpStatusCode.OK => Ok(result),
            HttpStatusCode.Created => Created("", result),
            _ => Ok(),
        };
    }

    protected async Task<IActionResult> NotFoundActionResult()
    {
        if (!OperationIsValid())
        {
            return await Errors();
        }

        return await Task.FromResult(NotFound());
    }

    protected async Task<IActionResult> NotFoundActionResult<T>(T result = default)
    {
        if (!OperationIsValid())
        {
            return await Errors();
        }


        return await Task.FromResult(NotFound(result));
    }

    protected async Task<IActionResult> UnauthorizedActionResult()
    {
        if (!OperationIsValid())
        {
            return await Errors();
        }


        return await Task.FromResult(new AuthenticationFailureResult());
    }

    protected async Task<IActionResult> UnauthorizedActionResult<T>(T result = default)
    {
        if (!OperationIsValid())
        {
            return await Errors();
        }


        return await Task.FromResult(new AuthenticationFailureResult<T>(result));
    }

    protected async Task<IActionResult> ForbiddenActionResult()
    {
        if (!OperationIsValid())
        {
            return await Errors();
        }


        return await Task.FromResult(Forbid());
    }

    protected async Task<IActionResult> BadRequestActionResult()
    {
        if (!OperationIsValid())
        {
            return await Errors();
        }


        return await Task.FromResult(BadRequest());
    }

    protected async Task<IActionResult> BadRequestActionResult<T>(T result = default)
    {
        if (!OperationIsValid())
        {
            return await Errors();
        }


        return await Task.FromResult(BadRequest(result));
    }

    protected async Task<IActionResult> UnprocessableEntityActionResult()
    {
        if (!OperationIsValid())
        {
            return await Errors();
        }


        return await Task.FromResult(UnprocessableEntity());
    }

    protected async Task<IActionResult> UnprocessableEntityActionResult<T>(T result = default)
    {
        if (!OperationIsValid())
        {
            return await Errors();
        }


        return await Task.FromResult(UnprocessableEntity(result));
    }

    protected async Task<IActionResult> NoContentActionResult()
    {
        if (!OperationIsValid())
        {
            return await Errors();
        }


        return await Task.FromResult(NoContent());
    }

    protected async Task<IActionResult> CreatedActionResult(string? uri = null, object? obj = null)
    {
        if (!OperationIsValid())
        {
            return await Errors();
        }


        return await Task.FromResult(Created(uri, obj));
    }

    protected async Task<IActionResult> OkActionResult()
    {
        if (!OperationIsValid())
        {
            return await Errors();
        }


        return await Task.FromResult(Ok());
    }

    protected async Task<IActionResult> OkActionResult<T>(T result = default)
    {
        if (!OperationIsValid())
        {
            return await Errors();
        }


        return await Task.FromResult(Ok(result));
    }

    protected void SpawnKeepConnectionAliveTask()
    {
        var counter = 0;
        var waitStep = 5000;
        var maxCounter = 600;
        var response = this.Response;
        var cancellationTokenSource = new CancellationTokenSource();

        var keepAliveTask = Task.Factory.StartNew(() =>
        {
            while (counter++ < maxCounter)
            {
                try
                {
                    response.WriteAsync("\r\n");
                    response.Clear();

                    // Sleep for the waitStep time before sending the next keep-alive message
                    Thread.Sleep(waitStep);

                    // You can log the keep-alive messages if needed.
                }
                catch (Exception e)
                {
                    if (e is BadHttpRequestException)
                    {
                        // Handle the exception if necessary.
                    }
                }
            }

            // If the counter reaches the maxCounter, cancel the task.
            cancellationTokenSource.Cancel();
        }, cancellationTokenSource.Token);

        // Optionally, you can handle the completion of the task or any exceptions that might occur.
        keepAliveTask.ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                // The task was canceled due to reaching the maxCounter.
            }
            else if (task.IsFaulted)
            {
                // The task encountered an exception. You can handle it here.
            }
            else
            {
                // The task completed successfully.
            }
        });
    }

    public void Dispose()
    {
        Notifications.ClearBySeesion();
    }
}
