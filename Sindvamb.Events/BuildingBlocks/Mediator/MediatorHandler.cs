#region

using FluentValidation;
using FluentValidation.Results;

using Microsoft.Extensions.Configuration;

#endregion

namespace Sindvamb.Events.BuildingBlocks.Mediator;

public class MediatorHandler : IMediatorHandler
{
    private readonly IEventSourcingRepository _eventSourcingRepository;
    public IMediator Mediator { get; set; }

    public MediatorHandler(IMediator mediator,
        IEventSourcingRepository eventSourcingRepository,
        IConfiguration configuration)
    {
        Mediator = mediator;
        _eventSourcingRepository = eventSourcingRepository;
        UseEventStore = configuration.GetValue<bool>("AppSettings:EventStore:Enabled");
    }

    public bool UseEventStore { get; set; }

    public async Task SendCommand<T>(T command) where T : ICommandMessage
    {
        if (await IsValid(command))
        {
            await Mediator.Send(command);
        }
    }

    public async Task<TResponse> SendCommand<TCommand, TResponse>(TCommand command)
        where TCommand : IRequest<TResponse>

    {
        try
        {
            if (await IsValid(command))
            {
                return await Mediator.Send(command);
            }

        }
        catch
        {
           //
        }

        return default!;
    }

    public async Task PublishEvent<T>(T domainEvent) where T : IEvent
    {
        await Mediator.Publish(domainEvent);

        if (UseEventStore && domainEvent?.AggregateId != null) await _eventSourcingRepository.Save(domainEvent!);
    }

    public async Task PublishNotification<T>(T notification) where T : IDomainNotification
    {
        await Mediator.Publish(notification);
    }

    public async Task PublishDomainEvent<T>(T notification) where T : IDomainEvent
    {
        await Mediator.Publish(notification);
    }

    public async Task<TResponse> SendQuery<TCommand, TResponse>(TCommand command)
        where TCommand : IBaseRequest, IRequest<TResponse>, new()
    {
        if (await IsValid(command))
        {
            return await Mediator.Send(command);
        }

        return default;
    }

    public async Task<TResponse> SendQuery<TCommand, TResponse, TValidator>(TCommand command)
        where TCommand : IBaseRequest, IRequest<TResponse>
        where TValidator : AbstractValidator<TCommand>, new()
    {
        if (await IsValid(command))
        {
            return await Mediator.Send(command);
        }

        return default;
    }


    public async Task<bool> IsValid(dynamic command)
    {
        if (!command.GetType().Name.Contains("Command") && !command.GetType().Name.Contains("Query")) return true;

        Type validatorType = null;
        var validatorName = string.Empty;

        if (command.GetType().Name.Contains("Command")) validatorName = command.GetType().Name.Replace("Command", "Validator");

        if (command.GetType().Name.Contains("Query")) validatorName = command.GetType().Name.Replace("Query", "Validator");

        if (validatorName.IsNullOrWhiteSpace()) return true;

        var AppDomainAssemblies = AppDomain.CurrentDomain.GetAssemblies();

        try
        {
            foreach (var Assembly in AppDomainAssemblies)
            {
                if (validatorType != null) break;

                validatorType = AssemblyHelper.GetAllTypes(Assembly)
                    .FirstOrDefault(type => type.IsNotNull() && type.Name.Contains(validatorName));
            }
        }
        catch 
        {
            //
        }

        if (validatorType == null) return true;

        var toValidate = validatorType.BaseType.GenericTypeArguments[0];

        if (validatorType.BaseType.GenericTypeArguments.Any() &&
            validatorType.BaseType.GenericTypeArguments[0].Name.Equals(command.GetType().Name))
        {
            return command.Validate(validatorType);
        }

        if (validatorType.BaseType.GenericTypeArguments.Any() &&
            validatorType.BaseType.GenericTypeArguments[0].Name.Equals(toValidate.Name))
        {
            try
            {
                dynamic inctance = Activator.CreateInstance(validatorType);
                ValidationResult validationResult = inctance.Validate(command.Input);
                var isValid = validationResult.IsValid && validationResult.Errors?.Count == 0;

                if (!isValid)
                {
                    await PublishNotification(new DomainNotification(validatorName, validationResult.Errors
                        .Select(e => new { e.PropertyName, e.ErrorMessage })
                        .ToList()));
                }

                return isValid;
            }
            catch
            {
            }
        }

        return true;
    }

    public async Task ErrorNotification(string msg, HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity)
    {
        await PublishNotification(new DomainNotification(msg, statusCode: statusCode));
    }

    public async Task<bool> Validate<TCommand, TValidator>(TCommand command)
        where TCommand : IBaseRequest
        where TValidator : AbstractValidator<TCommand>, new()
    {
        var validationResult = new ValidationResult();
        var validator = new TValidator();

        if (command != null) validationResult = validator.Validate(command!);

        if (validationResult is null) throw new Exception("Erro ao efetuar validação!");

        var isValid = validationResult.IsValid && validationResult.Errors?.Count == 0;

        if (!isValid)
            foreach (var error in validationResult.Errors!)
                await PublishNotification(new DomainNotification(nameof(ValidationResult),
                    error.ErrorMessage));

        return isValid;
    }
}