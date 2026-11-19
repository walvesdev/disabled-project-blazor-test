using MediatR;
using Sindvamb.Application.Core;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;

namespace Sindvamb.Application.Host.Controllers;

public class ContactController : AppControllerCrudBase<Contact, ContactDTO, Guid>
{
    public ContactController(
        INotificationHandler<DomainNotification> notifications,
        IMediatorHandler mediatorHandler) : base(notifications,
        mediatorHandler)
    {
        MediatorHandler = mediatorHandler;
    }
    
}