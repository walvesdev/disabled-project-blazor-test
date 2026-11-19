using MediatR;

using System.Collections.Generic;

using Sindvamb.Domain.DTO;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Users;
using Sindvamb.Utils.Attributes;

namespace Sindvamb.Service.Commands.Users
{
    [DependencyInjection(
        ServiceType = typeof(IRequestHandler<GetAllUnauthorizedUsersCommand, List<RegistrationRequestDTO>>),
        ImplementType = typeof(UserCommandHandler))
    ]
    public class GetAllUnauthorizedUsersCommand : Command<GetAllUnauthorizedUsersCommand, FilterDTO, List<RegistrationRequestDTO>>
    {
    }
}
