#region

using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Sindvamb.Security.Handlers;
using Sindvamb.Utils;
using Sindvamb.Utils.Configurations;
using AuthPolicy = Sindvamb.Security.Configurations.AuthPolicy;

#endregion

namespace Sindvamb.Application.Core.Auth;

public static class JwtConfigWebApi
{
    public static IServiceCollection AddJwtConfigWebApi(this IServiceCollection services,
        IConfiguration Configuration)
    {
        var signingConfigurations = new SigningConfigurations();
        services.AddSingleton(signingConfigurations);

        var tokenConfigurations = new TokenConfigurations();

        new ConfigureFromConfigurationOptions<TokenConfigurations>(
                Configuration.GetSection("AppSettings:TokenConfigurations"))
            .Configure(tokenConfigurations);

        signingConfigurations.SecurityKeySecret = tokenConfigurations.SecurityKeySecret;

        services.AddSingleton(tokenConfigurations);

        services.AddAuthentication("BasicAuthentication")
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null)
            .AddJwtBearer(bearerOptions =>
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = tokenConfigurations.Issuer,
                    ValidAudience = tokenConfigurations.Audience,
                    IssuerSigningKey = signingConfigurations.Key
                };

                bearerOptions.TokenValidationParameters = tokenValidationParameters;

                // Any code before the first await in this delegate can run
                // synchronously, so if you have events to attach for all requests
                // attach handlers before await.
                bearerOptions.Events = new JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        var token = context.HttpContext.Request.Headers["Authorization"];
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        // Ensure we always have an error and error description.
                        if (string.IsNullOrEmpty(context.Error))
                        {
                            context.Error = "invalid_token";
                        }

                        if (string.IsNullOrEmpty(context.ErrorDescription))
                        {
                            context.ErrorDescription = "This request requires a valid JWT access token to be provided";
                        }

                        // Add some extra context for expired tokens.
                        if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() ==
                            typeof(SecurityTokenExpiredException))
                        {
                            var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                            context.Response.Headers.Append("x-token-expired",
                                authenticationException!.Expires.ToString("o"));
                            context.ErrorDescription = $"The token expired on {authenticationException.Expires:o}";
                        }

                        return context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            error = context.Error,
                            error_description = context.ErrorDescription
                        }));
                    },

                    // This method is first event in authentication pipeline
                    // we have chance to wait until TokenValidationParameters
                    // is loaded.
                    OnMessageReceived = context =>
                    {

                        if (!context.HttpContext.User.Identity.IsAuthenticated)
                        {
                            // Wait until token validation parameters loaded.
                            var token = context.HttpContext.Request.Headers["Authorization"];

                            if (string.IsNullOrWhiteSpace(token))
                            {
                                return Task.CompletedTask;
                            }

                            token = token.ToString().Replace("Bearer ", "");

                            try
                            {
                                var tokenHandler = new JwtSecurityTokenHandler();
                                var key = Encoding.ASCII.GetBytes(tokenConfigurations.SecurityKeySecret);
                                tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);

                                var jwtToken = (JwtSecurityToken)validatedToken;
                            }
                            catch (Exception)
                            {
                                // ignored
                            }

                            context.Token = token;
                        }

                        return Task.CompletedTask;
                    }
                };
            });

        // Ativa o uso do token como forma de autorizar o acesso
        // a recursos deste projeto
        services.AddAuthorization(auth =>
        {
            var scopes = GetScopes();

            Array.ForEach(scopes, scope =>
                auth.AddPolicy(scope,
                    policy => policy.Requirements.Add(
                        new ScopeRequirement(tokenConfigurations.Issuer, scope)
                    )
                )
            );

            auth.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes("BasicAuthentication", JwtBearerDefaults.AuthenticationScheme)
                .RequireClaim(
                    "AppClaims",
                    AuthPolicy.ADMIN,
                    AuthPolicy.MASTER,
                    AuthPolicy.SUPPORT
                )
                .Build();

            auth.AddPolicy(AuthPolicy.MASTER, new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes("BasicAuthentication", JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().RequireRole(AuthPolicy.ADMIN).Build());

            auth.AddPolicy(AuthPolicy.MASTER, new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes("BasicAuthentication", JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().RequireRole(AuthPolicy.MASTER).Build());

            auth.AddPolicy(AuthPolicy.CUSTOMER, new AuthorizationPolicyBuilder()
               .AddAuthenticationSchemes("BasicAuthentication", JwtBearerDefaults.AuthenticationScheme)
               .RequireAuthenticatedUser().RequireRole(AuthPolicy.CUSTOMER).Build());

            auth.AddPolicy(AuthPolicy.SUPPORT, new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes("BasicAuthentication", JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().RequireRole(AuthPolicy.SUPPORT).Build());
        });


        return services;
    }

    private static string[] GetScopes()
    {
        return new[]
        {
            "read:billing_settings",
            "update:billing_settings",
            "read:customers",
            "read:files"
        };
    }
}
