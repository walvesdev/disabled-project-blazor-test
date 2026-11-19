#region

using Sindvamb.Domain.Entities;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Security.Configurations;
using Sindvamb.Utils;
using Sindvamb.Utils.Helpers;
using Sindvamb.Utils.Services.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using Sindvamb.Security.Enums;
using Sindvamb.Templates;
using Sindvamb.Templates.Pages.Usuarios;
using Sindvamb.Utils.Extension;
using Microsoft.Extensions.Options;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Utils.Configurations;
using Sindvamb.Domain.DTO;

#endregion

namespace Sindvamb.Security.Services;

public enum AuthenticationType
{
    Jwt,
    Basic
}

public class LoginService : ITransientDependency
{
    public LoginService(
        ISindvambRepository<User, Guid> userRepository,
        SigningConfigurations signingConfigurations,
        TokenConfigurations tokenConfigurations,
        IOptions<AppSettingsSection> options,
        ISindvambRepository<Customer, Guid> customerRepository, IMediatorHandler? mediatorHandler)
    {
        AppSettings = options.Value;
        UserRepository = userRepository;
        SigningConfigurations = signingConfigurations;
        TokenConfigurations = tokenConfigurations;
        CustomerRepository = customerRepository;
        MediatorHandler = mediatorHandler;
    }


    protected AppSettingsSection AppSettings { get; set; }

    public ISindvambRepository<User, Guid> UserRepository { get; set; }

    public ISindvambRepository<Customer, Guid> CustomerRepository { get; set; }

    public SigningConfigurations SigningConfigurations { get; set; }

    public TokenConfigurations TokenConfigurations { get; set; }

    public IMediatorHandler? MediatorHandler { get; set; }

    public async Task<dynamic> Login(LoginDTO dto, AuthenticationType authenticationType = AuthenticationType.Basic)
    {
        try
        {
            var user = new User();

            try
            {
                var cpf = dto.CpfCnpj.GetDigits();
                var pass = CryptoAES.EncryptStringToBytes(dto.Password);
                
                user = UserRepository.Model
                        .Where(u =>
                            u.CpfCnpj == cpf &&
                            u.Password == pass
                            )
                        .Select(u => new User
                        {
                            Id = u.Id,
                            Name = u.Name,
                            Email = u.Email,
                            Password = u.Password,
                            RoleId = u.RoleId,
                            Role = u.Role,
                            UserStatus = u.UserStatus,
                            IsCustomer = u.IsCustomer
                        })
                        .FirstOrDefault();

                if (user.IsNull())
                {
                    await MediatorHandler!.PublishNotification(new DomainNotification(nameof(LoginService),
                        "Falha ao autenticar, Credenciais inalidas!"));

                    return new BasicAccessToken
                    {
                        Authenticated = false,
                        Message = "Falha ao autenticar, Credenciais inalidas!"
                    };
                }

                if (user.Password.IsNullOrWhiteSpace())
                {
                    await MediatorHandler!.PublishNotification(new DomainNotification(nameof(LoginService),
                        "Erro ao autenticar, por favor solicite a alteração de senha!"));

                    return new BasicAccessToken
                    {
                        Authenticated = false,
                        Message = "Erro ao autenticar, por favor solicite a alteração de senha!"
                    };
                }
            }
            catch 
            {
                await MediatorHandler!.PublishNotification(new DomainNotification(nameof(LoginService),
                     "Erro ao efetuar autenticação, por favor contate o suporte!"));

                return new BasicAccessToken
                {
                    Authenticated = false,
                    Message = "Erro ao efetuar autenticação, por favor contate o suporte!"
                };
            }
            

            if (user.IsCustomer == true)
            {
                var customer = await CustomerRepository.FirstOrDefaultAsync(u => u.UserId == user.Id);

                if (customer.IsNull())
                {
                    await MediatorHandler!.PublishNotification(new DomainNotification(nameof(LoginService),
                        "Cliente não encontrado, contate o suporte para analisar seu cadastro!"));

                    return new BasicAccessToken
                    {
                        Authenticated = false,
                        Message = "Cliente não encontrado, contate o suporte para analisar seu cadastro!"
                    };
                }
            }

            if (user.UserStatus.Code.Equals("ACTIVATION_PENDING"))
            {
                await MediatorHandler!.PublishNotification(new DomainNotification(nameof(LoginService),
                    "Usuário pendente de ativação!"));

                return new BasicAccessToken
                {
                    Authenticated = false,
                    Message = "Usuário pendente de ativação!"
                };
            }

            if (user.UserStatus.Code.Equals("CHANGE_PASSWORD_NEEDED"))
            {
                await MediatorHandler!.PublishNotification(new DomainNotification(nameof(LoginService),
                    "Usuário com troca de senha requerida!"));

                return new BasicAccessToken
                {
                    Authenticated = false,
                    Message = "Usuário com troca de senha requerida!"
                };
            }

            if (user.UserStatus.Code.Equals("BLOCKED"))
            {
                await MediatorHandler!.PublishNotification(new DomainNotification(nameof(LoginService),
                    "Usuário bloqueado!"));

                return new BasicAccessToken
                {
                    Authenticated = false,
                    Message = "Usuário bloqueado!"
                };
            }

            if (user.UserStatus.Code.Equals("WAITING"))
            {
                await MediatorHandler!.PublishNotification(new DomainNotification(nameof(LoginService),
                    "Usuário aguardando liberação de acesso!"));

                return new BasicAccessToken
                {
                    Authenticated = false,
                    Message = "Usuário aguardando liberação de acesso!"
                };
            }
            
            if (user.UserStatus.Code.Equals("INACTIVE"))
            {
                await MediatorHandler!.PublishNotification(new DomainNotification(nameof(LoginService),
                    "Usuário inativo!"));

                return new BasicAccessToken
                {
                    Authenticated = false,
                    Message = "Usuário inativo!"
                };
            }

            var dataCriacao = DateTime.UtcNow.AddMinutes(5);
            var dataExpiracao = dataCriacao + TimeSpan.FromMinutes(TokenConfigurations.Lifetime);

            var scheme = JwtBearerDefaults.AuthenticationScheme;

            var identities = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.Name),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim("Email", user.Email),
                new Claim("Protocol", user.Matricula?.ToString() ?? ""),
                new Claim("Sub", user.Id.ToString()),
            };

            if (user.Role.IsNotNull())
            {
                identities.Add(new Claim(ClaimTypes.Role, user.Role.Key));
            }
            else
            {
                identities.Add(new Claim(ClaimTypes.Role, "GUEST"));
            }

            if (user.IsCustomer.HasValue && user.IsCustomer.Value)
            {
                var customer = CustomerRepository
                    .Where(c => c.UserId == user.Id)
                    .Include(c => c.CustomerType)
                    .FirstOrDefault();

                identities.Add(new Claim("CustomerType", customer.CustomerType.Code));
            }


            var identity = new ClaimsIdentity(
                new GenericIdentity(user!.Email, scheme),
                identities
            );

            var pricipal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(pricipal, scheme);
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Issuer = TokenConfigurations.Issuer,
                Audience = TokenConfigurations.Audience,
                NotBefore = dataCriacao,
                Expires = dataExpiracao,
                SigningCredentials = SigningConfigurations.SigningCredentials
            };

            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            scheme = "BasicAuthentication";

            if (authenticationType == AuthenticationType.Basic)
            {
                return new BasicAccessToken
                {
                    Authenticated = true,
                    AuthenticationTicket = ticket,
                    Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    Message = "OK",
                    Token = token,
                    Code = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Success = true
                };
            }

            if (authenticationType == AuthenticationType.Jwt)
            {
                return new JwtAccessToken
                {
                    Authenticated = true,
                    Created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    Expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss"),
                    Message = "OK",
                    Token = token,
                    Code = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Success = true
                };
            }

            return default;
        }
        catch
        {
            return false;
        }
    }

    public bool Validate(User user)
    {
        var userBase = new User
        {
            Email = "email@email.com",
            Password = "123",
        };

        return user.Email == userBase.Email &&
               user.Password == userBase.Password;
    }
}