#region

using FluentValidation;
using FluentValidation.Results;

using Microsoft.Extensions.Logging;

using Sindvamb.Events.BuildingBlocks.Common;
using Sindvamb.Events.BuildingBlocks.Mediator;

#endregion

namespace Sindvamb.Events.BuildingBlocks.Messages;

public abstract class CommandBase
{
    protected CommandBase()
    {
        Timestamp = DateTime.UtcNow;
    }

    public IMediatorHandler MediatorHandler { get; set; }


    public ILogger Logger { protected get; set; }

    public string MessageType => GetType().Name;

    public dynamic AggregateId { get; set; }

    public DateTime Timestamp { get; set; }

    public ValidationResult ValidationResult { get; set; }

    public object Data { get; set; }
}

public abstract class Command : CommandBase, ICommandMessage
{
    public IMediatorHandler MediatorHandler { get; set; }


    public ILogger Logger { protected get; set; }

    public string MessageType => GetType().Name;

    public dynamic AggregateId { get; set; }

    public DateTime Timestamp { get; set; }

    public ValidationResult ValidationResult { get; set; }

    public object Data { get; set; }

}

public abstract class Command<TResponse> : CommandBase, ICommandMessage<TResponse>
{
}

public abstract class Command<TCommand, TResponse> : CommandBase, ICommandMessage<TCommand, TResponse>//, IInpuTCommand<TCommand>
    where TCommand : IRequest<TResponse>
{
}

public abstract class Command<TCommand, TInput, TResponse> : CommandBase, ICommandMessage<TCommand, TResponse>, IInpuTCommand<TInput>
    where TCommand : IRequest<TResponse>
{
    public TInput? Input { get; set; }
}

public abstract class Command<TCommand, TResponse, TValidator, TInput> : CommandBase, ICommandMessage<TCommand, TResponse, TValidator, TInput>
    where TCommand : IBaseRequest, IRequest<TResponse>
    where TInput : class, new()
    where TValidator : AbstractValidator<TCommand>, new()
{
    //public async Task<bool> Validate(IBaseRequest request)
    //{
    //    if (request == null)
    //    {
    //        ValidationResult = new TValidator().Validate((TCommand)request!);
    //    }


    //    if (ValidationResult is null)
    //    {
    //        throw new Exception("Erro ao efetuar validação!");
    //    }
    //    var isValid = ValidationResult.IsValid && ValidationResult.Errors?.Count == 0;

    //    if (!isValid)
    //    {
    //        foreach (var error in ValidationResult.Errors!)
    //        {
    //            await MediatorHandler.PublishNotification(new DomainNotification(nameof(ValidationResult),
    //                error.ErrorMessage));
    //        }
    //    }

    //    return isValid;
    //}
}

public abstract class CommandHandler<TCommand> : CommandBase, ICommandHandler<TCommand>
    where TCommand : IRequest
{
    protected CommandHandler(IMediatorHandler mediatorHandler)
    {
        MediatorHandler = mediatorHandler;
    }

    public async Task Handle(TCommand request, CancellationToken cancellationToken) => await Execute(request, cancellationToken);

    protected abstract Task Execute(TCommand request, CancellationToken cancellationToken);
}

public abstract class CommandHandler<TCommand, TResponse> : CommandBase, ICommandHandler<TCommand, TResponse>
    where TCommand : IRequest<TResponse>
{
    protected CommandHandler(IMediatorHandler mediatorHandler)
    {
        MediatorHandler = mediatorHandler;
    }

    public async Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken) => await Execute(request, cancellationToken);

    protected abstract Task<TResponse> Execute(TCommand request, CancellationToken cancellationToken);
}
