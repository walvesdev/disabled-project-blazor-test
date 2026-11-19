using System;
using MediatR;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Customers;
using Sindvamb.Utils.Attributes;

namespace Sindvamb.Service.Commands.Customers
{
    [DependencyInjection(
        ServiceType = typeof(IRequestHandler<DeleteDependentComnand, bool>),
        ImplementType = typeof(CustomerCommandHandler))
    ]
    public class DeleteDependentComnand : Command<DeleteDependentComnand, Guid, bool>
    {
        public DeleteDependentComnand(Guid id) => Input = id;
    }
}
