using MediatR;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sindvamb.Application.Core;
using Sindvamb.Application.Core.FIlters;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Domain.Models;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Service.Commands.GenericCommands;
using Sindvamb.Service.Commands.Users;
using Sindvamb.Service.Inputs;

namespace Sindvamb.Application.Host.Controllers;

public class ContactRequestController : AppControllerCrudBase<ContactRequest, ContactRequestDTO, Guid>
{

    public ContactRequestController(
        INotificationHandler<DomainNotification> notifications,
        IMediatorHandler mediatorHandler) : base(notifications,
        mediatorHandler)
    {
        MediatorHandler = mediatorHandler;
    }
    
  
}