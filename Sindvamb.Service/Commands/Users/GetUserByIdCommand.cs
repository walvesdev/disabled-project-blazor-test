using MediatR;

using System;

using Sindvamb.Domain.DTO;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Users;
using Sindvamb.Utils.Attributes;

namespace Sindvamb.Service.Commands.Users
{
    [DependencyInjection(
        ServiceType = typeof(IRequestHandler<GetUserByIdCommand, UserDTO>),
        ImplementType = typeof(UserCommandHandler))
    ]
    public class GetUserByIdCommand : Command<GetUserByIdCommand, UserDTO>
    {
        public Guid Id { get; set; }
        public GetUserByIdCommand(Guid id) => Id = id;
    }
}
