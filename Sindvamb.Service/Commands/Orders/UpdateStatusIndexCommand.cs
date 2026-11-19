using MediatR;

using Sindvamb.Domain.DTO;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Orders;
using Sindvamb.Utils.Attributes;

namespace Sindvamb.Service.Commands.Orders;

[DependencyInjection(
    ServiceType = typeof(IRequestHandler<UpdateStatusIndexCommand, bool>),
    ImplementType = typeof(OrderCommandHandler))
]
public class UpdateStatusIndexCommand : Command<UpdateStatusIndexCommand, OrderDTO, bool>
{
    public UpdateStatusIndexCommand(OrderDTO input) => Input = input;
}
