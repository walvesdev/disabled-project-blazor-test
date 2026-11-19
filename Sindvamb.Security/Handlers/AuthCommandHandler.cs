#region

using AutoMapper;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Security.Commands.Users;
using Sindvamb.Security.Configurations;
using Sindvamb.Security.Services;
using Sindvamb.Utils;

#endregion

namespace Sindvamb.Security.Handlers;

public class AuthCommandHandler :
    ICommandHandler<LoginCommand, JwtAccessToken>
{

    private readonly IConfiguration _configuration;

    public AuthCommandHandler(
        IMediatorHandler mediatorHandler,
        IOptions<AppSettingsSection> options,
        IConfiguration configuration,
        LoginService loginService)
    {
        MediatorHandler = mediatorHandler;
        _configuration = configuration;
        LoginService = loginService;
        AppSettings = options.Value;
    }

    protected AppSettingsSection AppSettings { get; set; }

    public IMediatorHandler? MediatorHandler { get; set; }

    public IOptions<AppSettingsSection> Options { get; }

    public LoginService LoginService { get; set; }

    public IMapper Mapper { get; set; }

    public async Task<JwtAccessToken> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var login = command.Input;
        try
        {
            var result = await LoginService.Login(login, AuthenticationType.Jwt);
            return result;
        }
        catch
        {
            return default;
        }

    }
}