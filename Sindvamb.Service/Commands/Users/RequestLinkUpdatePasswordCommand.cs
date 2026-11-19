using MediatR;

using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Users;
using Sindvamb.Utils.Attributes;

namespace Sindvamb.Service.Commands.Users
{
    [DependencyInjection(
        ServiceType = typeof(IRequestHandler<RequestLinkUpdatePasswordCommand, bool>),
        ImplementType = typeof(UserCommandHandler))
    ]
    public class RequestLinkUpdatePasswordCommand : Command<RequestLinkUpdatePasswordCommand, string, bool>
    {
        public string CpfCnpj { get; set; }

        public RequestLinkUpdatePasswordCommand(string cpfCnpj)
        {
            CpfCnpj = cpfCnpj;
        }
    }
}
