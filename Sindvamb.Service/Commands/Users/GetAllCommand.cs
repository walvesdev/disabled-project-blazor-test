using MediatR;

using System.Collections.Generic;

using Sindvamb.Domain.DTO;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Users;
using Sindvamb.Utils.Attributes;

namespace Sindvamb.Service.Commands.Users
{
    [DependencyInjection(
        ServiceType = typeof(IRequestHandler<GetAllCommand, List<UserDTO>>),
        ImplementType = typeof(UserCommandHandler))
    ]
    public class GetAllCommand : Command<GetAllCommand, List<UserDTO>>
    {
    }
}
