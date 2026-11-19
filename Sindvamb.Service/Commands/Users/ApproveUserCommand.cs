using MediatR;

using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Users;
using Sindvamb.Utils.Attributes;

namespace Sindvamb.Service.Commands.Users
{
    [DependencyInjection(
        ServiceType = typeof(IRequestHandler<ApproveUserCommand, bool?>),
        ImplementType = typeof(UserCommandHandler))
    ]
    public class ApproveUserCommand : Command<ApproveUserCommand, RegistrationRequest, bool?>
    {
        public ApproveUserCommand(RegistrationRequest input) => Input = input;
    }
}
