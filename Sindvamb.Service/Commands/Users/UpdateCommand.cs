using MediatR;

using Sindvamb.Domain.DTO;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Users;
using Sindvamb.Utils.Attributes;

namespace Sindvamb.Service.Commands.Users
{
    [DependencyInjection(
        ServiceType = typeof(IRequestHandler<UpdateCommand, bool>),
        ImplementType = typeof(UserCommandHandler))
    ]
    public class UpdateCommand : Command<UpdateCommand, UserDTO, bool>
    {
        public UpdateCommand(UserDTO input) => Input = input;
    }
}
