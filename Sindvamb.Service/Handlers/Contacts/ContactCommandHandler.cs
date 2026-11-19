#region

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Service.Commands.GenericCommands;
using Sindvamb.Service.Inputs;
using Sindvamb.Service.Net;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;

#endregion

namespace Sindvamb.Service.Handlers.Contacts;

public class ContactCommandHandler :
    IRequestHandler<GenericCommand<List<ContactDTO>>, List<ContactDTO>>,
    IRequestHandler<GenericCommand<Guid, ContactDTO>, ContactDTO>,
    IRequestHandler<GenericCommand<CreateInput<ContactDTO>, bool>, bool>,
    IRequestHandler<GenericCommand<UpdateInput<ContactDTO>, bool>, bool>,
    IRequestHandler<GenericCommand<DeleteInput<Guid, Contact>, bool>, bool>
{
    private readonly IConfiguration _configuration;
    protected AppSettingsSection AppSettings { get; set; }
    private ISmtpClientHelper Email { get; set; }
    public IMediatorHandler? MediatorHandler { get; set; }

    public IOptions<AppSettingsSection> Options { get; }
    public ISindvambRepository<Contact, Guid> Repository { get; set; }
    public IMapper Mapper { get; set; }
    public ISindvambUnitOfWork UnitOfWork { get; set; }


    public ContactCommandHandler(
        IMediatorHandler mediatorHandler,
        IOptions<AppSettingsSection> options,
        IConfiguration configuration,
        IMapper mapper,
        ISindvambRepository<Contact, Guid> repository,
        ISmtpClientHelper email,
        ISindvambUnitOfWork unitOfWork)
    {
        MediatorHandler = mediatorHandler;
        _configuration = configuration;
        Mapper = mapper;
        Repository = repository;
        AppSettings = options.Value;
        Email = email;
        UnitOfWork = unitOfWork;
    }


    public async Task<List<ContactDTO>> Handle(GenericCommand<List<ContactDTO>> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var tt = await Repository
                .GetAllAsync(cancellationToken: cancellationToken);

            var list = await Repository
                .GetAll()
                .Include(c => c.Address)
                .ToListAsync(cancellationToken: cancellationToken);

            return list.MapTo<List<ContactDTO>>();
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateContactRequest",
                "Erro ao obter contatos. Se o erro persistir Contate o Suporte do sistema!"));
            
            return default;
        }
    }

    public async Task<ContactDTO> Handle(GenericCommand<Guid, ContactDTO> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = await Repository
                .GetAll()
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.Id.Equals(request.Input), cancellationToken: cancellationToken)!;

            return model.MapTo<ContactDTO>();
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateContactRequest",
                "Erro ao obter contato. Se o erro persistir Contate o Suporte do sistema!"));
            
            return default;
        }
    }

    public async Task<bool> Handle(GenericCommand<CreateInput<ContactDTO>, bool> request,
        CancellationToken cancellationToken)
    {
        try
        {
            await Repository.AddAsync(request.Input.Model.MapTo<Contact>(), true,
                cancellationToken: cancellationToken);

            return true;
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("CreateContact",
                "Erro ao criar contato. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<UpdateInput<ContactDTO>, bool> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = request.Input!.Model.MapTo<Contact>();
            ;
            await Repository.UpdateAsync(model, true, cancellationToken);

            return true;
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateContact",
                "Erro ao atulizar contato. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<DeleteInput<Guid, Contact>, bool> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = Repository.Find(request.Input.Id);

            if (model.IsNull())
            {
                await MediatorHandler!.PublishNotification(new DomainNotification("DeleteContact",
                    $"Contato não encontrado!"));
                return false;
            }

            await Repository.DeleteAsync(model, true, cancellationToken);
            return true;
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("DeleteContact",
                "Erro ao atulizar contato. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }
    }
}