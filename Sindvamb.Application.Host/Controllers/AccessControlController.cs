using MediatR;
using Sindvamb.Application.Core;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;

namespace Sindvamb.Application.Host.Controllers;

public class AccessControlController : AppControllerCrudBase<AccessControl, AccessControlDTO, Guid>
{
    public AccessControlController(
        INotificationHandler<DomainNotification> notifications,
        IMediatorHandler mediatorHandler) : base(notifications,
        mediatorHandler)
    {
        MediatorHandler = mediatorHandler;
    }
    

}