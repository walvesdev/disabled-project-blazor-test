using MediatR;

using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Users;
using Sindvamb.Utils.Attributes;

namespace Sindvamb.Service.Commands.Users
{
    [DependencyInjection(
        ServiceType = typeof(IRequestHandler<DenyUserCommand, bool>),
        ImplementType = typeof(UserCommandHandler))
    ]
    public class DenyUserCommand : Command<DenyUserCommand, RegistrationRequest, bool>
    {
        public DenyUserCommand(RegistrationRequest input) => Input = input;
    }
}
