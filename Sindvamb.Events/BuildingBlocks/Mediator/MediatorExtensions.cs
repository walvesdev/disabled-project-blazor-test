#region

#endregion

using FluentValidation;
using FluentValidation.Results;

using Hangfire;

namespace Sindvamb.Events.BuildingBlocks.Mediator;

public static class MediatorExtensions
{
    public static void Enqueue(this IMediatorHandler mediator, string jobName, IRequest request)
    {
        var client = new BackgroundJobClient();
        client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request));
    }

    public static void Enqueue(this IMediatorHandler mediator, IRequest request)
    {
        var client = new BackgroundJobClient();
        client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(request));
    }

    public static void Enqueue<TCommand>(this IMediatorHandler mediator, string jobName, IRequest<TCommand> request)
    {
        var client = new BackgroundJobClient();
        client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request));
    }

    public static void Enqueue<TCommand>(this IMediatorHandler mediator, IRequest<TCommand> request)
    {
        var client = new BackgroundJobClient();
        client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(request));
    }

    public static void Schedule(this IMediatorHandler mediator, string jobName, IRequest request, DateTimeOffset dateTimeOffset)
    {
        var client = new BackgroundJobClient();
        client.Schedule<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request), dateTimeOffset);
    }

    public static void Schedule(this IMediatorHandler mediator, IRequest request, DateTimeOffset dateTimeOffset)
    {
        var client = new BackgroundJobClient();
        client.Schedule<MediatorHangfireBridge>(bridge => bridge.Send(request), dateTimeOffset);
    }

    public static void Schedule<TCommand>(this IMediatorHandler mediator, string jobName, IRequest<TCommand> request, DateTimeOffset dateTimeOffset)
    {
        var client = new BackgroundJobClient();
        client.Schedule<MediatorHangfireBridge>(bridge => bridge.Send(jobName, request), dateTimeOffset);
    }

    public static void Schedule<TCommand>(this IMediatorHandler mediator, IRequest<TCommand> request, DateTimeOffset dateTimeOffset)
    {
        var client = new BackgroundJobClient();
        client.Schedule<MediatorHangfireBridge>(bridge => bridge.Send(request), dateTimeOffset);
    }

    public static bool ValidateFailure<TObject, TValiadator>(this TObject obj) where TValiadator : AbstractValidator<TObject>
    {
        try
        {
            dynamic inctance = Activator.CreateInstance(typeof(TValiadator));

            if (inctance == null) return true;

            var validationResult = inctance.Validate(obj);
            var isValid = validationResult.IsValid && validationResult.Errors?.Count == 0;

            if (!isValid)
            {
                var list = new List<ValidationFailure>();
                list.AddRange(validationResult.Errors as List<ValidationFailure>);
                ExceptionNotification.IsException = true;
                ExceptionNotification.SetNotification("Ocorreu Erros de Validação",
                    errors: list
                        .Select(e => new { e.PropertyName, e.ErrorMessage })
                        .ToList()
                );
            }

            return isValid;
        }
        catch
        {
            throw;
        }
    }

    public static bool ValidateFailure(this object obj, Type validator, dynamic objValidate)
    {
        try
        {
            dynamic inctance = Activator.CreateInstance(validator);

            if (inctance == null) return true;

            var validationResult = inctance.Validate(objValidate);
            var isValid = validationResult.IsValid && validationResult.Errors?.Count == 0;

            if (!isValid)
            {
                var list = new List<ValidationFailure>();
                list.AddRange(validationResult.Errors as List<ValidationFailure>);
                ExceptionNotification.IsException = true;
                ExceptionNotification.SetNotification("Ocorreu Erros de Validação",
                    errors: list
                        .Select(e => new { e.PropertyName, e.ErrorMessage })
                        .ToList()
                );
            }

            return isValid;
        }
        catch
        {
            throw;
        }
    }


}