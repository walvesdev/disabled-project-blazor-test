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
        ServiceType = typeof(IRequestHandler<GetFullCustomerComand, Customer>),
        ImplementType = typeof(CustomerCommandHandler))
    ]
    public class GetFullCustomerComand : Command<GetFullCustomerComand, Guid, Customer>
    {
        public GetFullCustomerComand(Guid id) => Input = id;
    }
}
