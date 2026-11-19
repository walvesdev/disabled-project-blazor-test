#region

using System;
using System.Collections.Generic;
using System.Linq;
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
using Sindvamb.Templates;
using Sindvamb.Templates.Pages.Usuarios;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;

#endregion

namespace Sindvamb.Service.Handlers.ContactRequestsRequest;

public class ContactRequestCommandHandler :
    IRequestHandler<GenericCommand<List<ContactRequestDTO>>, List<ContactRequestDTO>>,
    IRequestHandler<GenericCommand<Guid, ContactRequestDTO>, ContactRequestDTO>,
    IRequestHandler<GenericCommand<CreateInput<ContactRequestDTO>, bool>, bool>,
    IRequestHandler<GenericCommand<UpdateInput<ContactRequestDTO>, bool>, bool>,
    IRequestHandler<GenericCommand<DeleteInput<Guid, ContactRequest>, bool>, bool>
{
    private readonly IConfiguration _configuration;

    protected AppSettingsSection AppSettings { get; set; }

    private ISmtpClientHelper Email { get; set; }

    public IMediatorHandler? MediatorHandler { get; set; }

    public IOptions<AppSettingsSection> Options { get; }

    public ISindvambRepository<ContactRequest, Guid> Repository { get; set; }

    public IMapper Mapper { get; set; }

    public ISindvambUnitOfWork UnitOfWork { get; set; }


    public ContactRequestCommandHandler(
        IMediatorHandler mediatorHandler,
        IOptions<AppSettingsSection> options,
        IConfiguration configuration,
        IMapper mapper,
        ISindvambRepository<ContactRequest, Guid> repository,
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


    public async Task<List<ContactRequestDTO>> Handle(GenericCommand<List<ContactRequestDTO>> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var list = await Repository
                .GetAll()
                .Include(c => c.Contact)
                .ToListAsync(cancellationToken: cancellationToken);

            var result = list.MapTo<List<ContactRequestDTO>>();

            return result;
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateContactRequest",
                "Erro ao obter solicitações de contato. Se o erro persistir Contate o Suporte do sistema!"));

            return default;
        }
    }

    public async Task<ContactRequestDTO> Handle(GenericCommand<Guid, ContactRequestDTO> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var list = await Repository
                .GetAll()
                .Include(c => c.Contact)
                .FirstOrDefaultAsync(c => c.Id.Equals(request.Input), cancellationToken: cancellationToken)!;

            return list.MapTo<ContactRequestDTO>();
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateContactRequest",
                "Erro ao obter solicitação de contato. Se o erro persistir Contate o Suporte do sistema!"));

            return default;
        }
    }

    public async Task<bool> Handle(GenericCommand<CreateInput<ContactRequestDTO>, bool> request,
        CancellationToken cancellationToken)
    {
        try
        {
            _ = await UnitOfWork.Context.ContactRequest.AddAsync(request.Input!.Model.MapTo<ContactRequest>(), cancellationToken);
            await UnitOfWork.SaveChangesAsync(cancellationToken);

            var emailContact = AppServices.IsDevelopment() ?
                "tecnologia@sindvamb.com.br" :
                "contato@sindvamb.com.br";

            _ = await Email.SendGridAsync(
                "Solicitação de Contato - Sindvamb-DF",
                request.Input.Model.Contact.Email,
                request.Input.Model.Contact.Email,
                TemplateKey.REQUESTCONTACT,
                new RequestContact
                {
                    Name = request.Input.Model.Contact.Name,
                }, cancellationToken);

            _ = await Email.SendGridAsync(
                "Solicitação de Contato - Sindvamb-DF",
                emailContact,
                emailContact,
                TemplateKey.REQUESTCONTACTNOTIFY,
                new RequestContactNotify
                {
                    Request = request.Input.Model,
                }, cancellationToken);

            return true;
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("CreateContactRequest",
                "Erro ao criar solicitação de contato. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<UpdateInput<ContactRequestDTO>, bool> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var user = request.Input!.Model.Contact;

            var entity = await Repository
                .FirstOrDefaultAsync(f => f.Id == request.Input!.Model.Id, cancellationToken);

            if (entity.IsNotNull())
            {
                entity = request.Input!.Model.MapTo<ContactRequest>();
                entity.HasAnswered = true;
                entity.HasViewd = true;
                entity.AnsweredDate = DateTime.UtcNow;

                await Repository.UpdateAsync(entity, true, cancellationToken);

                _ = await Email.SendGridAsync(
                    "Solicitação de contato - Sindvamb-DF",
                    user.Email,
                    user.Email,
                    TemplateKey.REPLYCONTACTREQUEST,
                    new ReplyRequestContact
                    {
                        ContactRequest = entity
                    }, cancellationToken);

                return true;
            }

            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateContactRequest",
                "solicitação de contato não encontrada. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateContactRequest",
                "Erro ao atulizar solicitação de contato. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<DeleteInput<Guid, ContactRequest>, bool> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = Repository.Find(request.Input.Id);

            if (model.IsNull())
            {
                await MediatorHandler!.PublishNotification(new DomainNotification("DeleteContactRequest", $"Solicitação não encontrado!"));
                return false;
            }
            await Repository.DeleteAsync(model, true, cancellationToken);
            return true;
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("DeleteContactRequest",
                "Erro ao atulizar solicitação de contato. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }
    }
}