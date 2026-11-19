#region

using FluentValidation;

using Sindvamb.Events.BuildingBlocks.Mediator;

#endregion

namespace Sindvamb.Events.BuildingBlocks.Messages;

public interface ICommunication : IMessage
{

}

public interface ICommandMessage : ICommunication, IBaseRequest
{
}

public interface ICommandMessage<out TResponse> : ICommandMessage, IRequest<TResponse>
{
}

public interface ICommandMessage<TCommand, out TResponse> : ICommandMessage<TResponse>
    where TCommand : IRequest<TResponse>
{
}

public interface ICommandMessage<TCommand, out TResponse, TValidator, TInput> : ICommandMessage<TCommand, TResponse>
    where TValidator : AbstractValidator<TCommand>
    where TInput : class, new()
    where TCommand : IRequest<TResponse>
{
}

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand>
    where TCommand : IRequest
{
    public IMediatorHandler MediatorHandler { get; set; }
}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    where TCommand : IRequest<TResponse>
{
    public IMediatorHandler MediatorHandler { get; set; }
}

public interface IQueryMessage<TCommand, TResponse> : ICommandMessage<TCommand, TResponse>, ICommandQuery
    where TCommand : IRequest<TResponse>
{
}

public interface
    IQueryMessage<TCommand, TResponse, TValidator, TInput> : ICommandMessage<TCommand, TResponse, TValidator, TInput>,
        ICommandQuery
    where TCommand : IBaseRequest, IRequest<TResponse>
    where TInput : class, new()
    where TValidator : AbstractValidator<TCommand>, new()
{
}

public interface IQueryHandler<TCommand> : ICommandHandler<TCommand>
    where TCommand : IRequest
{
    public IMediatorHandler MediatorHandler { get; set; }
}

public interface IQueryHandler<TCommand, TResponse> : ICommandHandler<TCommand, TResponse>
    where TCommand : IRequest<TResponse>
{
    public IMediatorHandler MediatorHandler { get; set; }
}