#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using log4net.Core;

using MailKit.Search;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Org.BouncyCastle.Ocsp;
using Org.BouncyCastle.Utilities.Collections;

using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Domain.Enums;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Files.Pdf;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Security.Enums;
using Sindvamb.Service.Commands.Customers;
using Sindvamb.Service.Commands.GenericCommands;
using Sindvamb.Service.Commands.Users;
using Sindvamb.Service.Inputs;
using Sindvamb.Service.Net;
using Sindvamb.Templates;
using Sindvamb.Templates.Pages.Usuarios;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;
using Sindvamb.Utils.Helpers;

using static System.Runtime.InteropServices.JavaScript.JSType;

#endregion

namespace Sindvamb.Service.Handlers.Users;

public class UserCommandHandler :
    ICommandHandler<ApproveUserCommand, bool?>,
    ICommandHandler<DenyUserCommand, bool>,
    ICommandHandler<RecoveryPasswordCommand, bool>,
    ICommandHandler<UpdatePasswordCommand, bool>,
    ICommandHandler<RegisterCommand, bool>,
    ICommandHandler<UpdateCommand, bool>,
    ICommandHandler<DeleteCommand, bool>,
    ICommandHandler<GenericCommand<CreateInput<UserDTO>, bool>, bool>,
    ICommandHandler<GetAllCommand, List<UserDTO>>,
    ICommandHandler<GetUserByCpfCnpjCommand, Customer>,
    ICommandHandler<GetUserByIdCommand, UserDTO>,
    IRequestHandler<GetUnauthorizedUserStatusCommand, RegistrationRequestDTO>,
    ICommandHandler<GenericCommand<List<UserStatusDTO>>, List<UserStatusDTO>>,
    ICommandHandler<GetAllUnauthorizedUsersCommand, List<RegistrationRequestDTO>>,
    IRequestHandler<GenericCommand<List<RoleDTO>>, List<RoleDTO>>,
    IRequestHandler<RequestLinkUpdatePasswordCommand, bool>
{
    private readonly IConfiguration _configuration;
    protected AppSettingsSection AppSettings { get; set; }

    private ISmtpClientHelper Email { get; set; }

    public IMediatorHandler? MediatorHandler { get; set; }

    public IOptions<AppSettingsSection> Options { get; }

    public ISindvambRepository<User, Guid> Repository { get; set; }

    public ISindvambRepository<PasswordHistory, Guid> PasswordHistoryRepository { get; set; }

    public ISindvambRepository<CustomerType, Guid> CustomerTypeRepository { get; set; }

    public ISindvambRepository<Customer, Guid> CustomerRepository { get; set; }

    public ISindvambRepository<UserStatus, Guid> UserStatusRepository { get; set; }

    public ISindvambRepository<Role, Guid> RoleRepository { get; set; }
    public ISindvambRepository<FileControl, Guid> FileControlRepository { get; set; }
    

    public ISindvambRepository<RegistrationRequest, Guid> RegistrationRequestRepository { get; set; }

    public PdfGenerator PdfGenerator { get; set; }

    public IMapper Mapper { get; set; }

    public ISindvambUnitOfWork UnitOfWork { get; set; }
    public ILogger<UserCommandHandler> Logger { get; set; }


    public UserCommandHandler(
        IMediatorHandler mediatorHandler,
        IOptions<AppSettingsSection> options,
        IConfiguration configuration,
        IMapper mapper,
        ISindvambRepository<User, Guid> repository,
        ISindvambRepository<RegistrationRequest, Guid> registrationRequestRepository,
        ISindvambRepository<Role, Guid> roleRepository,
        ISindvambRepository<UserStatus, Guid> userStatusRepository,
        ISindvambRepository<CustomerType, Guid> customerTypeRepository,
        ISindvambRepository<Customer, Guid> customerRepository, ISmtpClientHelper email,
        ISindvambRepository<PasswordHistory, Guid> passwordHistoryRepository, ISindvambUnitOfWork unitOfWork, ILogger<UserCommandHandler> logger, PdfGenerator pdfGenerator, ISindvambRepository<FileControl, Guid> fileControlRepository)
    {
        MediatorHandler = mediatorHandler;
        _configuration = configuration;
        Mapper = mapper;
        Repository = repository;
        RegistrationRequestRepository = registrationRequestRepository;
        RoleRepository = roleRepository;
        AppSettings = options.Value;
        UserStatusRepository = userStatusRepository;
        CustomerTypeRepository = customerTypeRepository;
        CustomerRepository = customerRepository;
        Email = email;
        PasswordHistoryRepository = passwordHistoryRepository;
        UnitOfWork = unitOfWork;
        Logger = logger;
        PdfGenerator = pdfGenerator;
        FileControlRepository = fileControlRepository;
    }


    public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var userId = SequentialGuidGenerator.NewSequentialGuid();

            var protocol = $"{userId.ToString().GetDigits()[0..8].PadLeft(8, '0')}{DateTime.UtcNow:yyyy}";

            var reg = new RegistrationRequest
            {
                Approved = false,
                Protocol = protocol,
                User = new User
                {
                    Id = userId,
                    Matricula = request.Input.Matricula,
                    CpfCnpj = request.Input.CpfCnpj,
                    IsCustomer = true,
                    IsSystem = false,
                    Name = request.Input.Name,
                    Password = CryptoAES.EncryptStringToBytes(request.Input.Password),
                    Email = request.Input.Email,
                    RoleId = RoleRepository.FirstOrDefault(r => r.Key.Equals(Enum.GetName(EAuthRole.CUSTOMER))).Id,
                    PartnerUnitId = request.Input.PartnerUnitId,
                    UserStatusId = UserStatusRepository.FirstOrDefault(r => r.Code.Equals(Enum.GetName(EUserStatus.WAITING))).Id
                }
            };

             var customer = new Customer
             {
                 UserId = userId,
                 CustomerTypeId = CustomerTypeRepository.FirstOrDefault(c => c.Code.Contains("COM")).Id
             };

            if (Repository.Any(u => u.CpfCnpj == request.Input.CpfCnpj))
            {
                var regDb = await RegistrationRequestRepository.FirstOrDefaultAsync(c => c.UserId == userId);

                if (regDb.IsNotNull())
                {
                    regDb = reg.MapTo<RegistrationRequest>();
                    await RegistrationRequestRepository.UpdateAsync(regDb, true, cancellationToken);
                }
                else
                {
                    await RegistrationRequestRepository.AddAsync(reg, true, cancellationToken);
                }

                var cusDb = await CustomerRepository.FirstOrDefaultAsync(c => c.UserId == userId);

                if (cusDb.IsNotNull())
                {
                    cusDb = customer.MapTo<Customer>();
                    await CustomerRepository.UpdateAsync(cusDb, true, cancellationToken);
                }
                else
                {
                    await CustomerRepository.AddAsync(customer, true, cancellationToken);
                }               

                return true;
            }           

            await RegistrationRequestRepository.AddAsync(reg, true, cancellationToken);           
            await CustomerRepository.AddAsync(customer, true, cancellationToken);

            return true;
        }
        catch(Exception e)
        {
            return false;
        }
    }

    public async Task<List<UserDTO>> Handle(GetAllCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await Repository
                .GetAll()
                .Include(u => u.UserStatus)
                .Select(u => u.GetUserWithouFilesStream().MapTo<UserDTO>())
                .ToListAsync(cancellationToken);

            result.ForEach(u => u.Password = null);

            return result;
        }
        catch
        {
            return default;
        }
    }

    public async Task<bool> Handle(DeleteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await UnitOfWork.BeginTransactionAsync(cancellationToken);

            var ctx = UnitOfWork.Context;

            var user = await ctx.User.FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken: cancellationToken);

            if (user.IsNull())
            {
                await MediatorHandler.PublishNotification(new DomainNotification(nameof(RegisterCommand),
                    "Usuário não encontrado!"));

                return false;
            }

            var registrationRequest = await ctx.RegistrationRequest.FirstOrDefaultAsync(r => r.UserId == request.Id, cancellationToken: cancellationToken);

            if (registrationRequest.IsNotNull())
            {
                ctx.RegistrationRequest.Remove(registrationRequest);
            }

            var customer = await ctx.Customer.FirstOrDefaultAsync(r => r.UserId == request.Id, cancellationToken: cancellationToken);

            if (customer.IsNotNull())
            {
                ctx.Customer.Remove(customer);
            }

            ctx.User.Remove(user);

            await UnitOfWork.SaveChangesAsync(cancellationToken);
            await UnitOfWork.CommitTransactionAsync(cancellationToken);

            return true;
        }
        catch
        {
            await UnitOfWork.RollbackTransactionAsync(cancellationToken);

            await MediatorHandler.PublishNotification(new DomainNotification(nameof(RegisterCommand),
                "Erro ao excluir Usuário!"));

            return false;
        }
    }

    public async Task<List<RegistrationRequestDTO>> Handle(GetAllUnauthorizedUsersCommand request,
        CancellationToken cancellationToken)
    {
        var result = await RegistrationRequestRepository
            .GetAll()
            .Include(r => r.User)
            .Select(r => new RegistrationRequestDTO
            {
                Id = r.Id,
                UserId = r.User.Id,
                Protocol = r.Protocol,
                Approved = r.Approved,
                Reason = r.Reason,
                Date = r.Date,
                User = r.User.GetUserWithouFilesStream().MapTo<UserDTO>()
            })
            .ToListAsync();

        result.ForEach(u => u.User.Password = null);

        return result;
    }

    public async Task<UserDTO> Handle(GetUserByIdCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await Repository
                .Where(u => u.Id == request.Id)
                .Include(u => u.Address)
                .Include(u => u.FileControl)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (user.IsNull())
            {
                await MediatorHandler.PublishNotification(new DomainNotification(nameof(UpdateCommand),
                    "Usuário não encontrado!"));

                return null;
            }

            user.FileControl.RemoveAll(f => !f.Description.Contains("3X4"));

            user.Password = null;

            return user.MapTo<UserDTO>();
        }
        catch (Exception e)
        {
            await MediatorHandler.PublishNotification(new DomainNotification(nameof(UpdateCommand),
                "Erro ao buscar Usuário."));

            return null;
        }
    }

    public async Task<bool> Handle(UpdateCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await UnitOfWork.BeginTransactionAsync(cancellationToken);

            var user = await UnitOfWork.Repository<User, Guid>()
                .Where(p => p.Id == request.Input.Id)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (!request.Input.Password.IsNullOrWhiteSpace())
            {
                var history = new PasswordHistory();

                history.NewPassword = CryptoAES.EncryptStringToBytes(request.Input.Password);
                history.OldPassword = user.Password;
                history.HasChanged = true;
                history.UserId = user.Id;
                history.SecurityCode = SequentialGuidGenerator.NewSequentialGuid().ToString();

                request.Input.Password = history.NewPassword;
                request.Input.PasswordResetToken = null;

                await UnitOfWork.Repository<PasswordHistory, Guid>().AddAsync(history, false, cancellationToken);
            }
            else
            {
                request.Input.Password = user.Password;
            }

            user = request.Input.MapTo<User>();

            user.EducationDegree = null;
            user.PartnerUnit = null;
            user.UserStatus = null;
            user.EducationDegree = null;
            user.PartnerUnit = null;
            user.Role = null;
            user.UserStatus = null;

            user.CpfCnpj = user.CpfCnpj.GetDigits();

            await UnitOfWork.Repository<User, Guid>().UpdateAsync(user, false, cancellationToken);

            await UnitOfWork.SaveChangesAsync(cancellationToken);
            await UnitOfWork.CommitTransactionAsync(cancellationToken);

            return true;
        }
        catch (Exception e)
        {
            await UnitOfWork.RollbackTransactionAsync(cancellationToken);

            await MediatorHandler.PublishNotification(new DomainNotification(nameof(UpdateCommand),
                "Erro ao atualizar Usuário."));

            return false;
        }
    }

    public async Task<List<UserStatusDTO>> Handle(GenericCommand<List<UserStatusDTO>> request,
        CancellationToken cancellationToken)
    {
        return await UserStatusRepository.GetAll().Select(us => us.MapTo<UserStatusDTO>())
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<List<RoleDTO>> Handle(GenericCommand<List<RoleDTO>> request, CancellationToken cancellationToken)
    {
        return await RoleRepository.GetAll().Select(us => us.MapTo<RoleDTO>())
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> Handle(GenericCommand<CreateInput<UserDTO>, bool> request,
        CancellationToken cancellationToken)
    {
        try
        {
            //if (Repository.Any(u => u.CpfCnpj == request.Input.Model.CpfCnpj))
            //{
            //    await MediatorHandler.PublishNotification(new DomainNotification(nameof(CreateInput<UserDTO>),
            //        "Já existe um usuário cadastrado com esse numero de CPF ou CNPJ!"));
            //    return false;
            //}

            await UnitOfWork.BeginTransactionAsync(cancellationToken);

            var user = request.Input.MapTo<User>();
            user.IsCustomer = true;
            user.IsSystem = false;

            user.EducationDegree = null;
            user.PartnerUnit = null;
            user.UserStatus = null;
            user.EducationDegree = null;
            user.PartnerUnit = null;
            user.Role = null;
            user.UserStatus = null;

            user.CpfCnpj = user.CpfCnpj.GetDigits();

            await UnitOfWork.Repository<User, Guid>().UpdateAsync(user, false, cancellationToken);

            await UnitOfWork.SaveChangesAsync(cancellationToken);
            await UnitOfWork.CommitTransactionAsync(cancellationToken);

            return true;
        }
        catch (Exception e)
        {
            await UnitOfWork.RollbackTransactionAsync(cancellationToken);

            await MediatorHandler.PublishNotification(new DomainNotification(nameof(RegisterCommand),
                "Erro ao adicionar Usuário."));

            return false;
        }
    }

    private async Task Recovery(string email)
    {
        try
        {
            var user = Repository.FirstOrDefault(u => u.Email.Equals(email));

            if (user.IsNull())
            {
                return;
            }

            var history = new PasswordHistory
            {
                UserId = user.Id,
                OldPassword = user.Password,
                SecurityCode = SequentialGuidGenerator.NewSequentialGuid().ToString(),
                HasChanged = false
            };

            PasswordHistoryRepository.Add(history, true);

            var model = new RecoveryPassword
            {
                Nome = user.Name,
                Url = $"update-password/{history.SecurityCode}",
                FrontEndUrl = AppSettings.FrontEndUrl
            };

            var result = await Email.SendSmtpAsync(
                $"Recuperação de Senha - Sindvamb",
                user.Email,
                TemplateKey.RECOVERYPASSWORD,
                model
            );
        }
        catch
        {
            return;
        }
    }

    public async Task<bool> Handle(RecoveryPasswordCommand request, CancellationToken cancellationToken)
    {
        await Recovery(request.Input);
        return true;
    }

    public async Task<bool> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await Repository
                .GetAll(u => u.CpfCnpj.Equals(request.CpfCnpj.GetDigits()), trackEntities: true)
                .Include(u => u.PasswordHistory)
                .FirstOrDefaultAsync();

            if (user.IsNotNull())
            {
                var history = user.PasswordHistory.FirstOrDefault(p => p.SecurityCode == request.SecurityCode);

                if (history.IsNotNull())
                {
                    if (!history!.SecurityCode!.Equals(user.PasswordResetToken?.ToString()) || history.HasChanged)
                    {
                        await MediatorHandler.PublishNotification(new DomainNotification(nameof(ApproveUserCommand),
                            "O Token de seguraça é invalido!"));
                        return false;
                    }
                    else
                    {
                        history.NewPassword = CryptoAES.EncryptStringToBytes(request.Password);
                        history.HasChanged = true;

                        user.Password = history.NewPassword;
                        user.PasswordResetToken = null;

                        await Repository.UpdateAsync(user, true, cancellationToken);
                        return true;
                    }
                }
            }

            return false;
        }
        catch(Exception e)
        {
            await MediatorHandler.PublishNotification(new DomainNotification(nameof(ApproveUserCommand),
                "O Token de seguraça é invalido!"));
            return false;
        }
    }

    public async Task<RegistrationRequestDTO> Handle(GetUnauthorizedUserStatusCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var list = await RegistrationRequestRepository
                .GetAll()
                .Include(r => r.User)
                .ThenInclude(f => f.FileControl)
                .Include(r => r.User)
                .ThenInclude(f => f.UserStatus)
                .Include(r => r.User)
                .ThenInclude(f => f.EducationDegree)
                .Include(r => r.User)
                .ThenInclude(f => f.Address)
                .Include(r => r.User)
                .ThenInclude(f => f.SpecialNeeds)
                .Select(r => new RegistrationRequest
                {
                    Id = r.Id,
                    UserId = r.UserId,
                    User = r.User.GetUserWithouFilesStream(),
                    Approved = r.Approved,
                    Reason = r.Reason,
                    Protocol = r.Protocol,
                    Date = r.Date,

                }).ToListAsync();

            var registration = list.FirstOrDefault(u => u.User.CpfCnpj.Equals(request.Model.CpfCnpj.GetDigits()));

            if (registration is not null)
            {

                var reg = registration.MapTo<RegistrationRequestDTO>();

                var customer = await CustomerRepository
                    .GetAll(cus => cus.UserId == registration.UserId)
                    .Include(c => c.Dependents)
                    .ThenInclude(f => f.FileControl)
                    .Select(d => d.GetFilesWithouFilesStream())
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                reg.Dependents = customer.Dependents.MapTo<List<DependentDTO>>();

                reg.User.Password = null;

                return reg;
            }

            return null;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    public async Task<bool> Handle(DenyUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await UnitOfWork.BeginTransactionAsync(cancellationToken);

            var ctx = UnitOfWork.Context;

            var reg = await ctx.RegistrationRequest.FindAsync(request.Input!.Id, cancellationToken);

            reg.Approved = false;
            reg.Date = DateTime.UtcNow;
            reg.Reason = request.Input.Reason;

            ctx.RegistrationRequest.Update(reg);

            var user = await ctx.User.FindAsync(request.Input.UserId, cancellationToken);

            user.PasswordResetToken = null;
            user.UserStatusId = UserStatusRepository
                .FirstOrDefault(r => r.Code.Equals(Enum.GetName(EUserStatus.BLOCKED))).Id;

            ctx.User.Update(user);

            var customer = await ctx.Customer.FirstOrDefaultAsync(c => c.UserId == user.Id, cancellationToken);

            if (customer.IsNotNull())
            {

                ctx.Customer.Update(customer);
            }

            await UnitOfWork.SaveChangesAsync(cancellationToken);
            await UnitOfWork.CommitTransactionAsync(cancellationToken);

            try
            {
                _ = await Email.SendGridAsync(
                       "Solicitação de cadastro não aprovada - Sindvamb-DF",
                       user.Email,
                       user.Email,
                       TemplateKey.REGISTRATIONREQUESTDENY,
                       new RegistrationRequestEmailDeny
                       {
                           Nome = user.Name,
                           Email = user.Email,
                           CpfCnpj = user.CpfCnpj,
                           Protocolo = request.Input.Protocol,
                           Motivo = request.Input.Reason
                       }, cancellationToken);
            }
            catch (Exception e)
            {
                _ = await Email.SendSimpleSmtpAsync($"Erro Encontrado - Sindvamb", "tecnologia@sindvamb.com.br", e.Serialize());

            }

            return true;
        }
        catch (Exception e)
        {
            await UnitOfWork.RollbackTransactionAsync(cancellationToken);

            await MediatorHandler!.PublishNotification(new DomainNotification(nameof(ApproveUserCommand),
                $"Erro ao reprovar usuario, por favor contate o suporte!"));

            _ = await Email.SendSimpleSmtpAsync($"Erro Encontrado - Sindvamb", "tecnologia@sindvamb.com.br", e.Serialize());

            Console.WriteLine(e.StackTrace);
            Logger.LogError(e.Message);
            Logger.LogError(e.StackTrace);

            return false;
        }
    }

    public async Task<bool?> Handle(ApproveUserCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await UnitOfWork.BeginTransactionAsync(cancellationToken);

            var reg = await UnitOfWork.Repository<RegistrationRequest, Guid>()
                .FindAsync(request.Input!.Id, cancellationToken);

            reg.Approved = true;
            reg.Date = DateTime.UtcNow;
            reg.Reason = null;

            request.Input.User = null;

            await UnitOfWork.Repository<RegistrationRequest, Guid>()
                .UpdateAsync(reg, false, cancellationToken: cancellationToken);

            var user = await UnitOfWork.Repository<User, Guid>()
                .Where(u => u.Id == request.Input.UserId)
                .Include(u => u.PasswordHistory)
                .Include(u => u.Address)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            user.PasswordResetToken = Guid.NewGuid();
            user.UserStatusId = UserStatusRepository
                .FirstOrDefault(r => r.Code.Equals(Enum.GetName(EUserStatus.ACTIVE))).Id;

            if (user.Password.IsNullOrWhiteSpace())
            {
                user.Password = CryptoAES.EncryptStringToBytes("1q2w3E*");
            }

            var passHistory = new PasswordHistory
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                OldPassword = user.Password,
                NewPassword = user.Password,
                SecurityCode = user.PasswordResetToken.ToString(),
                HasChanged = false,
            };

            await UnitOfWork.Repository<PasswordHistory, Guid>()
                .AddAsync(passHistory, false, cancellationToken: cancellationToken);

            await UnitOfWork.Repository<User, Guid>()
                .UpdateAsync(user, false, cancellationToken: cancellationToken);

            var customer = await UnitOfWork.Repository<Customer, Guid>()
                .FirstOrDefaultAsync(c => c.UserId == user.Id, cancellationToken);

            if (customer.IsNotNull())
            {
                await UnitOfWork.Repository<Customer, Guid>()
                    .UpdateAsync(customer, false, cancellationToken: cancellationToken);
            }

            var cpfText = user.CpfCnpj.Length > 11 ? "CNPJ Nº" : "CPF Nº";

            var htmlAss = await TemplateHelper.GetHtml(TemplateKey.DECLARACAOASSOCIACAO, new DeclaracaoAssociacao
            {
                Name = user.Name,
                CpfCnpj = $"{cpfText} {Formaters.FormatCpfCnpj(user.CpfCnpj)}",
                RG = $"{user.RG} SSP {user.UFIssuingBody}",
                Endereco = $"{user.Address?.AddressLine1} {user.Address?.Number} {user.Address?.Neighborhood} - CEP: {Formaters.FormatCEP(user.Address?.ZipCode)} {user.Address?.City}-{user.Address?.UF}"
            });
            
            UnitOfWork.Repository<FileControl, Guid>()
                .Delete(f => f.UserId == reg.UserId && (f.FileName.Contains("Declaração Sesc.pdf") || f.FileName.Contains("Declaração de Associação.pdf")), false);

            var pdfAss = await PdfGenerator.Create(htmlAss, "Declaração de Associação", true);

            await UnitOfWork.Repository<FileControl, Guid>().AddAsync(new FileControl
            {
                FileName = $"Declaração de Associação.pdf",
                Description = $"Declaração de Associação.pdf",
                ContentType = "application/pdf",
                FileSize = pdfAss.Length,
                FileArray = pdfAss,
                UserId = user.Id,
            });

            var htmlSesc = await TemplateHelper.GetHtml(TemplateKey.DECLARACAOSESC, new DeclaracaoSesc
            {
                Name = user.Name,
            });

            var pdfSesc = await PdfGenerator.Create(htmlSesc, "Declaração de Sesc", true);

            await UnitOfWork.Repository<FileControl, Guid>().AddAsync(new FileControl
            {
                FileName = $"Declaração Sesc.pdf",
                Description = $"Declaração Sesc.pdf",
                ContentType = "application/pdf",
                FileSize = pdfSesc.Length,
                FileArray = pdfSesc,
                UserId = user.Id,
            });

            await UnitOfWork.SaveChangesAsync(cancellationToken);
            await UnitOfWork.CommitTransactionAsync(cancellationToken);

            _ = await Email.SendGridAsync(
                "Solicitação de cadastro Aprovada - Sindvamb-DF",
                user.Email,
                user.Email,
                TemplateKey.REGISTRATIONREQUESTAPPROVED,
                new RegistrationRequestEmailApproved
                {
                    Nome = user.Name,
                    Email = user.Email,
                    CpfCnpj = user.CpfCnpj,
                    Protocolo = request.Input.Protocol,
                    SecurityCode = passHistory.SecurityCode,
                }, cancellationToken);

            return request.Input.Approved;
        }
        catch (Exception e)
        {
            await UnitOfWork.RollbackTransactionAsync(cancellationToken);

            await MediatorHandler!.PublishNotification(new DomainNotification(nameof(ApproveUserCommand),
                "Erro ao atualizar usuario, por favor contate o suporte! " + e.Serialize()));
            return false;
        }
    }

    public async Task<Customer> Handle(GetUserByCpfCnpjCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var cpf = request.CpfCnpj.GetDigits();

            var user = Repository.FirstOrDefault(u => u.CpfCnpj.Equals(cpf));

            if (user.IsNotNull())
            {
                var customer = await MediatorHandler.SendCommand<GetFullCustomerComand, Customer>(new GetFullCustomerComand(user.Id));

                return customer;
            }

            return null;
        }
        catch (Exception e)
        {
            await MediatorHandler.PublishNotification(new DomainNotification(nameof(ApproveUserCommand),
                "Erro ao buscar usuario, por favor contate o suporte!"));
            return null;
        }
    }

    public async Task<bool> Handle(RequestLinkUpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await UnitOfWork.BeginTransactionAsync(cancellationToken);

            var user = await UnitOfWork.Repository<User, Guid>()
                .Where(u => u.CpfCnpj == request.CpfCnpj.GetDigits())
                .Include(u => u.PasswordHistory)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            if (user.IsNotNull())
            {
                user.PasswordResetToken = Guid.NewGuid();

                var passHistory = new PasswordHistory
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    OldPassword = user.Password,
                    NewPassword = user.Password,
                    SecurityCode = user.PasswordResetToken.ToString(),
                    HasChanged = false,
                };

                await UnitOfWork.Repository<PasswordHistory, Guid>()
                    .AddAsync(passHistory, false, cancellationToken: cancellationToken);

                await UnitOfWork.Repository<User, Guid>()
                    .UpdateAsync(user, false, cancellationToken: cancellationToken);

                await UnitOfWork.SaveChangesAsync(cancellationToken);
                await UnitOfWork.CommitTransactionAsync(cancellationToken);

                _ = await Email.SendGridAsync(
                    "Redefinição de senha - Sindvamb-DF",
                    user.Email,
                    user.Email,
                    TemplateKey.RECOVERYPASSWORDREQUEST,
                    new RecoveryPasswordRequest
                    {
                        Nome = user.Name,
                        Email = user.Email,
                        CpfCnpj = user.CpfCnpj,
                        SecurityCode = passHistory.SecurityCode,
                    }, cancellationToken);

                return true;
            }

            await MediatorHandler!.PublishNotification(new DomainNotification(nameof(ApproveUserCommand),
                "Usuário não encontrado, por favor contate o suporte!"));

            return false;
        }
        catch (Exception e)
        {
            await UnitOfWork.RollbackTransactionAsync(cancellationToken);

            await MediatorHandler!.PublishNotification(new DomainNotification(nameof(ApproveUserCommand),
                "Erro ao atualizar usuario, por favor contate o suporte!"));

            return false;
        }
    }
}