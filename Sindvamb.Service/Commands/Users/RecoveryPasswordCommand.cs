using MediatR;

using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Users;
using Sindvamb.Utils.Attributes;

namespace Sindvamb.Service.Commands.Users
{
    [DependencyInjection(
        ServiceType = typeof(IRequestHandler<RecoveryPasswordCommand, bool>),
        ImplementType = typeof(UserCommandHandler))
    ]
    public class RecoveryPasswordCommand : Command<RecoveryPasswordCommand, string, bool>
    {
        public RecoveryPasswordCommand(string email) => Input = email;
    }
}
