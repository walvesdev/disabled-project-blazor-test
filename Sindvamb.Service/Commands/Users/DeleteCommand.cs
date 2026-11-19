using MediatR;

using System;

using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Users;
using Sindvamb.Utils.Attributes;

namespace Sindvamb.Service.Commands.Users
{
    [DependencyInjection(
    ServiceType = typeof(IRequestHandler<DeleteCommand, bool>),
    ImplementType = typeof(UserCommandHandler))
]
    public class DeleteCommand : Command<DeleteCommand, Guid, bool>
    {
        public Guid Id { get; set; }
        public DeleteCommand(Guid id) => Id = id;
    }
}
