#region

using FluentValidation;

using Sindvamb.Events.BuildingBlocks.CommonMessages.DomainEvents;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Messages;

#endregion

namespace Sindvamb.Events.BuildingBlocks.Mediator;

public interface IMediatorHandler
{
    public IMediator Mediator { get; set; }
    Task SendCommand<T>(T command) where T : ICommandMessage;

    Task<TResponse> SendCommand<TCommand, TResponse>(TCommand command)
        where TCommand : IRequest<TResponse>; //where TCommand : ICommandMessage<TResponse>;

    Task PublishEvent<T>(T domainEvent) where T : IEvent;

    Task PublishDomainEvent<T>(T notification) where T : IDomainEvent;

    Task PublishNotification<T>(T notification) where T : IDomainNotification;

    Task<TResponse> SendQuery<TCommand, TResponse>(TCommand command)
        where TCommand : IBaseRequest, IRequest<TResponse>, new();

    Task<TResponse> SendQuery<TCommand, TResponse, TValidator>(TCommand command)
        where TCommand : IBaseRequest, IRequest<TResponse>
        where TValidator : AbstractValidator<TCommand>, new();

    Task<bool> Validate<TCommand, TValidator>(TCommand command)
        where TCommand : IBaseRequest
        where TValidator : AbstractValidator<TCommand>, new();

    Task<bool> IsValid(dynamic command);
    // where TCommand : IBaseRequest;

    Task ErrorNotification(string msg, HttpStatusCode statusCode = HttpStatusCode.UnprocessableEntity);
}
