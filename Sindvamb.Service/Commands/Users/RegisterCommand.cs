using MediatR;

using Sindvamb.Domain.DTO;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Users;
using Sindvamb.Utils.Attributes;

namespace Sindvamb.Service.Commands.Users
{
    [DependencyInjection(
    ServiceType = typeof(IRequestHandler<RegisterCommand, bool>),
    ImplementType = typeof(UserCommandHandler))
]
    public class RegisterCommand : Command<RegisterCommand, UserDTO, bool>
    {
        public RegisterCommand(UserDTO input) => Input = input;
    }
}
