using MediatR;

using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Service.Handlers.Users;
using Sindvamb.Utils.Attributes;

namespace Sindvamb.Service.Commands.Users
{
    [DependencyInjection(
        ServiceType = typeof(IRequestHandler<UpdatePasswordCommand, bool>),
        ImplementType = typeof(UserCommandHandler))
    ]
    public class UpdatePasswordCommand : Command<UpdatePasswordCommand, string, bool>
    {
        public string Password { get; set; }
        public string SecurityCode { get; set; }
        public string CpfCnpj { get; set; }

        public UpdatePasswordCommand(string password, string securityCode, string cpfCnpj)
        {
            Password = password;
            SecurityCode = securityCode;
            CpfCnpj = cpfCnpj;
        }
    }
}
