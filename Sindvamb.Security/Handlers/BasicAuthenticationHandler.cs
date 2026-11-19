#region

using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Security.Services;
using Sindvamb.Utils;
using Sindvamb.Utils.Configurations;
using Sindvamb.Utils.Extension;

#endregion

namespace Sindvamb.Security.Handlers;

public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public LoginService LoginService { get; }
    public SigningConfigurations SigningConfigurations { get; set; }
    public TokenConfigurations TokenConfigurations { get; set; }

    public BasicAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        LoginService loginService,
        SigningConfigurations signingConfigurations,
        TokenConfigurations tokenConfigurations) : base(options, logger, encoder, clock)
    {
        LoginService = loginService;
        SigningConfigurations = signingConfigurations;
        TokenConfigurations = tokenConfigurations;
    }

    protected override async Task<AuthenticateResult?> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return default;
        }

        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);

        try
        {
            if (authHeader.Parameter != null && authHeader.Scheme.Contains("Basic"))
            {
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);

                var password = credentials[1];
                var cpfCnpj = credentials[0];

                if (cpfCnpj.IsNull() || string.IsNullOrWhiteSpace(password))
                {
                    return AuthenticateResult.Fail("INVALID_ACCOUNT");
                }

                var token = await LoginService.Login(new LoginDTO()
                {
                    CpfCnpj = cpfCnpj,
                    Password = password
                }, AuthenticationType.Basic);

                Request.Headers["Authorization"] = $"Bearer {token.Token}";

                if (!token.Authenticated)
                {
                    return AuthenticateResult.Fail("Invalid Authorization");
                }

                return token.AuthenticationTicket != null
                    ? AuthenticateResult.Success(token.AuthenticationTicket)
                    : AuthenticateResult.Fail("Invalid Authorization");
            }
            else
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = TokenConfigurations.Issuer,
                    ValidAudience = TokenConfigurations.Audience,
                    IssuerSigningKey = SigningConfigurations.Key
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var principal = tokenHandler.ValidateToken(authHeader.Parameter, tokenValidationParameters, out var validatedToken);

                var scheme = JwtBearerDefaults.AuthenticationScheme;
                var ticket = new AuthenticationTicket(principal, scheme);

                return ticket != null
                    ? AuthenticateResult.Success(ticket)
                    : AuthenticateResult.Fail("Invalid Authorization");
            }
        }
        catch
        {
            // return AuthenticateResult.Fail(
            //     "Invalid Authorization Header (Expected Base64: \"email(string):password(string)\"");
            return AuthenticateResult.NoResult();
        }
    }
}