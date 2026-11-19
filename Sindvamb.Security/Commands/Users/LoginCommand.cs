#region

using MediatR;

using Sindvamb.Domain.DTO;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Security.Configurations;
using Sindvamb.Security.Handlers;
using Sindvamb.Utils.Attributes;

#endregion

namespace Sindvamb.Security.Commands.Users;

[DependencyInjection(
    ServiceType = typeof(IRequestHandler<LoginCommand, JwtAccessToken>),
    ImplementType = typeof(AuthCommandHandler))
]
public class LoginCommand : Command<LoginCommand, JwtAccessToken>
{
    public LoginCommand(LoginDTO input)
    {
        Input = input;
    }

    public LoginDTO Input { get; }
}
