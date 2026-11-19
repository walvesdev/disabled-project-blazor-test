using MediatR;

using Sindvamb.Domain.DTO;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Customers;
using Sindvamb.Utils.Attributes;

using System;
using Sindvamb.Domain.Entities;

namespace Sindvamb.Service.Commands.Customers
{
    [DependencyInjection(
        ServiceType = typeof(IRequestHandler<CreateDependentComnand, bool>),
        ImplementType = typeof(CustomerCommandHandler))
    ]
    public class CreateDependentComnand : Command<CreateDependentComnand, Guid, bool>
    {
        public DependentDTO Dependent { get; set; }
        public CreateDependentComnand(DependentDTO dto) => Dependent = dto;
    }
}
