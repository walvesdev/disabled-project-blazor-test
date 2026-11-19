using MediatR;

using System.Collections.Generic;

using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Models;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Users;
using Sindvamb.Utils.Attributes;

namespace Sindvamb.Service.Commands.Users
{
    [DependencyInjection(
        ServiceType = typeof(IRequestHandler<GetUnauthorizedUserStatusCommand, RegistrationRequestDTO>),
        ImplementType = typeof(UserCommandHandler))
    ]
    public class GetUnauthorizedUserStatusCommand(AcompanharCadastroModel model) : Command<GetUnauthorizedUserStatusCommand, FilterDTO, RegistrationRequestDTO>
    {
        public AcompanharCadastroModel Model { get; } = model;
    }
}
