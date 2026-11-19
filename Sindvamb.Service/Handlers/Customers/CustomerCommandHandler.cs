#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;
using Castle.Core.Resource;
using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Repository.ORM;
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

#endregion

namespace Sindvamb.Service.Handlers.Customers;

public class CustomerCommandHandler :
    ICommandHandler<GenericCommand<List<CustomerDTO>>, List<CustomerDTO>>,
    ICommandHandler<GenericCommand<Guid, CustomerDTO>, CustomerDTO>,
    ICommandHandler<GenericCommand<CreateInput<CustomerDTO>, CustomerDTO>, CustomerDTO>,
    ICommandHandler<GenericCommand<UpdateInput<CustomerDTO>, bool>, bool>,
    ICommandHandler<GenericCommand<DeleteInput<Guid, Customer>, bool>, bool>,
    IRequestHandler<GetFullCustomerComand, Customer>,
    IRequestHandler<CreateDependentComnand, bool>,
    IRequestHandler<DeleteDependentComnand, bool>
{
    private readonly IConfiguration _configuration;

    protected AppSettingsSection AppSettings { get; set; }

    public IMediatorHandler? MediatorHandler { get; set; }


    public IOptions<AppSettingsSection> Options { get; }

    public ISindvambRepository<Customer, Guid> Repository { get; set; }

    public ISindvambRepository<User, Guid> UserRepository { get; set; }

    public ISindvambRepository<Role, Guid> RoleRepository { get; set; }

    public ISindvambRepository<PartnerUnit, Guid> PartnerUnitRepository { get; set; }

    public ISindvambRepository<UserStatus, Guid> UserStatusRepository { get; set; }

    public ISindvambRepository<Dependent, Guid> DependentRepository { get; set; }

    public ISindvambRepository<RegistrationRequest, Guid> RegistrationRequestRepository { get; set; }

    public ISindvambRepository<CustomerType, Guid> CustomerTypeRepository { get; set; }
    public ISindvambUnitOfWork UnitOfWork { get; set; }

    public IMapper Mapper { get; set; }

    private ISmtpClientHelper Email { get; set; }

    public CustomerCommandHandler(
        IMediatorHandler mediatorHandler,
        IOptions<AppSettingsSection> options,
        IConfiguration configuration,
        IMapper mapper,
        ISindvambRepository<Customer, Guid> repository,
        ISindvambRepository<PartnerUnit, Guid> partnerUnitRepository,
        ISindvambRepository<RegistrationRequest, Guid> registrationRequestRepository,
        ISindvambRepository<Role, Guid> roleRepository,
        ISindvambRepository<UserStatus, Guid> userStatusRepository,
        ISindvambRepository<CustomerType, Guid> customerTypeRepository,
        ISindvambUnitOfWork unitOfWork,
        ISindvambRepository<User, Guid> userRepository, ISmtpClientHelper email, ISindvambRepository<Dependent, Guid> dependentRepository)
    {
        MediatorHandler = mediatorHandler;
        _configuration = configuration;
        Mapper = mapper;
        Repository = repository;
        UserRepository = userRepository;
        Email = email;
        AppSettings = options.Value;
        CustomerTypeRepository = customerTypeRepository;
        RegistrationRequestRepository = registrationRequestRepository;
        PartnerUnitRepository = partnerUnitRepository;
        RoleRepository = roleRepository;
        UserStatusRepository = userStatusRepository;
        UnitOfWork = unitOfWork;
        DependentRepository = dependentRepository;
    }


    public async Task<List<CustomerDTO>> Handle(GenericCommand<List<CustomerDTO>> request,
        CancellationToken cancellationToken)
    {
        return await Repository
            .GetAll()
            .Include(p => p.CustomerType)
            .Include(p => p.User)
            .ThenInclude(p => p.PartnerUnit)
            .ThenInclude(p => p.Partner)
            .Select(p => p.GetFilesWithouFilesStream().MapTo<CustomerDTO>())
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<CustomerDTO> Handle(GenericCommand<Guid, CustomerDTO> request,
        CancellationToken cancellationToken)
    {
        var customer = await Repository
            .Where(p => p.Id.Equals(request.Input))
            .Include(p => p.User)
            .ThenInclude(p => p.Address)
            .Include(p => p.CustomerType)
            .Include(p => p.User)
            .ThenInclude(p => p.PartnerUnit)
            .ThenInclude(p => p.Partner)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        return customer.GetFilesWithouFilesStream().MapTo<CustomerDTO>();
    }

    public async Task<CustomerDTO> Handle(GenericCommand<CreateInput<CustomerDTO>, CustomerDTO> request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (UserRepository.Any(u => u.CpfCnpj == request.Input.Model.User.CpfCnpj))
            {
                //await MediatorHandler.PublishNotification(new DomainNotification(nameof(RegisterCommand),
                //    "Já existe um usuário cadastrado com esse numero de CPF/CNPJ."));
                return null;
            }

            await UnitOfWork.BeginTransactionAsync(cancellationToken);

            var customer = request.Input.Model.MapTo<Customer>();
            customer.CustomerType = null;
            customer.CustomerOrder = null;
            customer.User.EducationDegree = null;
            customer.User.PartnerUnit = null;
            customer.User.UserStatus = null;
            customer.User.EducationDegree = null;
            customer.User.PartnerUnit = null;
            customer.User.Role = null;
            customer.User.UserStatus = null;

            customer.User.IsCustomer = true;
            customer.User.IsSystem = false;
            customer.User.Password = CryptoAES.EncryptStringToBytes("123456");

            //customer.User.SpecialNeeds = null;
            customer.UserId = customer.User.Id;
            customer.User.CpfCnpj = customer.User.CpfCnpj.GetDigits();
            customer.User.Password = CryptoAES.EncryptStringToBytes("1q2w3E*");

            customer.CustomerTypeId = UnitOfWork.Repository<CustomerType, Guid>()
                .FirstOrDefault(c => c.Code.Contains("SISTEMA_COMERCIO")).Id;
            customer.User.PartnerUnitId = UnitOfWork.Repository<PartnerUnit, Guid>().FirstOrDefault()?.Id;
            customer.User.RoleId = UnitOfWork.Repository<Role, Guid>()
                .FirstOrDefault(role => role.Key.Equals("CUSTOMER"))?.Id;
            customer.User.UserStatusId = UnitOfWork.Repository<UserStatus, Guid>()
                .FirstOrDefault(status => status.Code.Equals("WAITING"))?.Id;


            if (request.Input.Model.IsRegistrationRequest)
            {
                var protocol = $"{customer.User.Id.ToString().GetDigits()[0..8].PadLeft(8, '0')}{DateTime.UtcNow:yyyy}";

                await UnitOfWork.Repository<RegistrationRequest, Guid>().AddAsync(new RegistrationRequest
                {
                    Approved = null,
                    UserId = customer.UserId,
                    Protocol = protocol,
                    Date = DateTime.UtcNow
                }, false, cancellationToken);
            }


            if (customer.User.Address.IsNotNull())
            {
                //customer.User.Address.Id = Guid.NewGuid();
                await UnitOfWork.Repository<Address, Guid>().AddAsync(customer.User.Address, false, cancellationToken);

            }

            await UnitOfWork.Repository<Customer, Guid>().AddAsync(customer, false, cancellationToken);

            await UnitOfWork.SaveChangesAsync(cancellationToken);
            await UnitOfWork.CommitTransactionAsync(cancellationToken);

            return customer.MapTo<CustomerDTO>();
        }
        catch (Exception e)
        {
            await UnitOfWork.RollbackTransactionAsync(cancellationToken);

            await MediatorHandler.PublishNotification(new DomainNotification(nameof(RegisterCommand),
                "Erro ao cadastrar cliente."));

            return null;
        }
    }

    public async Task<bool> Handle(GenericCommand<UpdateInput<CustomerDTO>, bool> request,
        CancellationToken cancellationToken)
    {
        try
        {
            await UnitOfWork.BeginTransactionAsync(cancellationToken);

            var customer = new Customer();

            if (request.Input.Model.User.IsCustomer == true)
            {
                customer = await UnitOfWork.Repository<Customer, Guid>()
                    .Where(p => p.UserId == request.Input.Model.UserId)
                    .Include(p => p.Dependents)
                    .Include(p => p.User)
                    .ThenInclude(p => p.FileControl)
                    .Include(p => p.User)
                    .ThenInclude(p => p.Address)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);


                if (customer.User.FileControl.Any())
                {
                    customer.User.FileControl = null;
                }

                if (customer.Dependents.Any())
                {
                    foreach (var dependent in customer.Dependents)
                    {
                        dependent.FileControl = null;
                    }                   

                }

                request.Input.Model.Id = customer.Id;

                //var pass = customer.User.Password;

                customer = request.Input.Model.MapTo<Customer>();
                customer.User.IsCustomer = true;
                customer.User.IsSystem = false;

                customer.CustomerType = null;
                customer.CustomerOrder = null;
                customer.User.EducationDegree = null;
                customer.User.PartnerUnit = null;
                customer.User.UserStatus = null;
                customer.User.EducationDegree = null;
                customer.User.PartnerUnit = null;
                customer.User.Role = null;
                customer.User.UserStatus = null;

                customer.CustomerTypeId = UnitOfWork.Repository<CustomerType, Guid>()
               .FirstOrDefault(c => c.Code.Contains("SISTEMA_COMERCIO")).Id;

                customer.User.CpfCnpj = customer.User.CpfCnpj.GetDigits();

                await UnitOfWork.Repository<Customer, Guid>().UpdateAsync(customer, false, cancellationToken);
            }
            else
            {
                var user = await UnitOfWork.Repository<User, Guid>()
                    .GetAll()
                    .Include(f => f.FileControl)
                    .Include(f => f.UserStatus)
                    .Include(f => f.EducationDegree)
                    .Include(f => f.Address)
                    .Include(f => f.SpecialNeeds)
                    .Include(f => f.PartnerUnit)
                    .ThenInclude(f => f.Partner)
                    .FirstOrDefaultAsync(u => u.Id.Equals(request.Input.Model.Id),
                        cancellationToken: cancellationToken);

                if (user.FileControl.Any())
                {
                    user.FileControl = null;
                }

                user = request.Input?.Model?.User?.MapTo<User>();
                user.EducationDegree = null;
                user.PartnerUnit = null;
                user.UserStatus = null;
                user.EducationDegree = null;
                user.PartnerUnit = null;
                user.Role = null;
                user.UserStatus = null;
                await UnitOfWork.Repository<User, Guid>().UpdateAsync(user, false, cancellationToken);
            }

            await UnitOfWork.SaveChangesAsync(cancellationToken);
            await UnitOfWork.CommitTransactionAsync(cancellationToken);

            if (request.Input.Model.IsRegistrationRequest)
            {
                try
                {
                    var reg = UnitOfWork.Repository<RegistrationRequest, Guid>().GetAll().FirstOrDefault(r => r.UserId == request.Input.Model.UserId);

                    var emailContact = AppServices.IsDevelopment() ?
                        "tecnologia@sindvamb.com.br" :
                        "contato@sindvamb.com.br";

                    _ = await Email.SendGridAsync(
                        "Bem Vindo ao Sindicato dos Ambulantes DF",
                        request.Input.Model.User.Email,
                        request.Input.Model.User.Name,
                        TemplateKey.REGISTRATIONREQUEST,
                        new RegistrationRequestEmail
                        {
                            Nome = request.Input.Model.User.Name,
                            Email = request.Input.Model.User.Email!,
                            CpfCnpj = request.Input.Model.User.CpfCnpj,
                            Protocolo = reg.Protocol
                        }, cancellationToken);

                    _ = await Email.SendGridAsync(
                        "Solicitação de cadastro Sindvamb-DF",
                        emailContact,
                        request.Input.Model.User.Name,
                        TemplateKey.REGISTRATIONREQUESTNOTIFY,
                        new RegistrationRequestEmailNotify
                        {
                            Nome = request.Input.Model.User.Name,
                            Email = request.Input.Model.User.Email!,
                            CpfCnpj = request.Input.Model.User.CpfCnpj,
                            Protocolo = reg.Protocol
                        }, cancellationToken);
                }
                catch (Exception e)
                {

                    await UnitOfWork.RollbackTransactionAsync(cancellationToken);

                    await MediatorHandler.PublishNotification(new DomainNotification(nameof(RegisterCommand),
                        "Erro ao atualizar cliente."));

                    return false;
                }
            }

            return true;
        }
        catch (Exception e)
        {
            await UnitOfWork.RollbackTransactionAsync(cancellationToken);

            await MediatorHandler.PublishNotification(new DomainNotification(nameof(RegisterCommand),
                "Erro ao atualizar cliente."));

            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<DeleteInput<Guid, Customer>, bool> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var client = Repository.Find(request.Input.Id);

            if (client.IsNull())
            {
                await MediatorHandler.PublishNotification(new DomainNotification("Delete Cliente",
                    $"Cliente não encontrado!"));
                return false;
            }

            await Repository.DeleteAsync(client, true, cancellationToken);
            await UserRepository.DeleteAsync(client.UserId, true, cancellationToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<Customer> Handle(GetFullCustomerComand request, CancellationToken cancellationToken)
    {
        try
        {
            var customer = await Repository
                .GetAll()
                .Include(r => r.CustomerType)
                .Include(r => r.Dependents)
                .ThenInclude(f => f.FileControl)
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
                .Include(r => r.User)
                .ThenInclude(f => f.PartnerUnit)
                .FirstOrDefaultAsync(u =>
                    u.User.Id.Equals(request.Input), cancellationToken: cancellationToken);

            if (customer.IsNull())
            {
                customer = new Customer();

                customer.User = await UserRepository
                    .GetAll()
                    .Include(f => f.FileControl)
                    .Include(f => f.UserStatus)
                    .Include(f => f.EducationDegree)
                    .Include(f => f.Address)
                    .Include(f => f.SpecialNeeds)
                    .Include(f => f.PartnerUnit)
                    .ThenInclude(f => f.Partner)
                    .FirstOrDefaultAsync(u => u.Id.Equals(request.Input), cancellationToken: cancellationToken);

                customer.User.IsCustomer = false;
            }

            customer.User.Password = null;
            return customer.GetFilesWithouFilesStream();
        }
        catch (Exception e)
        {
            await MediatorHandler.PublishNotification(new DomainNotification("GetFullCustomerComand",
                $"Cliente não encontrado!"));
            return null;
        }
    }

    public async Task<bool> Handle(CreateDependentComnand request, CancellationToken cancellationToken)
    {
        try
        {
            var customerId = Repository.FirstOrDefault(c => c.UserId == request.Dependent.UserId).Id;

            var dependent = await DependentRepository.FindAsync(request.Dependent.Id);

            if (dependent.IsNotNull())
            {
                dependent = request.Dependent.MapTo<Dependent>();
                dependent.CustomerId = customerId;
                await DependentRepository.UpdateAsync(dependent, true, cancellationToken);
            }
            else
            {
                dependent = request.Dependent.MapTo<Dependent>();
                dependent.Customer = null;
                dependent.CustomerId = customerId;
                await DependentRepository.AddAsync(dependent, true, cancellationToken);
            }

            return true;
        }
        catch (Exception e)
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("CreateDependentComnand",
                "Erro ao atulizar depentente. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }
    }

    public async Task<bool> Handle(DeleteDependentComnand request, CancellationToken cancellationToken)
    {
        try
        {
            var client = await DependentRepository.FindAsync(request.Input);

            if (client.IsNull())
            {
                await MediatorHandler.PublishNotification(new DomainNotification("DeleteDependentComnand",
                    $"Dependente não encontrado!"));
                return false;
            }

            await DependentRepository.DeleteAsync(client, true, cancellationToken);

            return true;
        }
        catch
        {
            await MediatorHandler.PublishNotification(new DomainNotification("DeleteDependentComnand",
                    $"Dependente não encontrado!"));

            return false;
        }
    }
}