using MediatR;

using System.Collections.Generic;

using Sindvamb.Domain.DTO;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Users;
using Sindvamb.Utils.Attributes;
using Sindvamb.Domain.Entities;

namespace Sindvamb.Service.Commands.Users
{
    [DependencyInjection(
        ServiceType = typeof(IRequestHandler<GetUserByCpfCnpjCommand, Customer>),
        ImplementType = typeof(UserCommandHandler))
    ]
    public class GetUserByCpfCnpjCommand : Command<GetUserByCpfCnpjCommand, Customer>
    {
        public string CpfCnpj { get; set; }

        public GetUserByCpfCnpjCommand(string cpfCnpj)
        {
            CpfCnpj = cpfCnpj;
        }
    }
}
