#region

using AutoMapper;

using MediatR;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using Sindvamb.Domain.DTO;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Service.Commands.GenericCommands;
using Sindvamb.Service.Inputs;
using Sindvamb.Service.Net;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AccessControl = Sindvamb.Domain.Entities.AccessControl;

#endregion

namespace Sindvamb.Service.Handlers.AccessControls;

public class AccessControlCommandHandler :
    IRequestHandler<GenericCommand<List<AccessControlDTO>>, List<AccessControlDTO>>,
    IRequestHandler<GenericCommand<Guid, AccessControlDTO>, AccessControlDTO>,
    IRequestHandler<GenericCommand<CreateInput<AccessControlDTO>, bool>, bool>,
    IRequestHandler<GenericCommand<UpdateInput<AccessControlDTO>, bool>, bool>,
    IRequestHandler<GenericCommand<DeleteInput<Guid, AccessControl>, bool>, bool>
{
    private readonly IConfiguration _configuration;
    protected AppSettingsSection AppSettings { get; set; }
    private ISmtpClientHelper Email { get; set; }
    public IMediatorHandler? MediatorHandler { get; set; }
    public IOptions<AppSettingsSection> Options { get; }
    public ISindvambRepository<AccessControl, Guid> Repository { get; set; }
    public IMapper Mapper { get; set; }
    public ISindvambUnitOfWork UnitOfWork { get; set; }


    public AccessControlCommandHandler(
        IMediatorHandler mediatorHandler,
        IOptions<AppSettingsSection> options,
        IConfiguration configuration,
        IMapper mapper,
        ISindvambRepository<AccessControl, Guid> repository,
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


    public async Task<List<AccessControlDTO>> Handle(GenericCommand<List<AccessControlDTO>> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var list = await Repository
                .GetAll()
                .ToListAsync(cancellationToken: cancellationToken);

            return list.MapTo<List<AccessControlDTO>>();
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateAccessControlRequest",
                "Erro ao obter acessos. Se o erro persistir Contate o Suporte do sistema!"));

            return default;
        }
    }

    public async Task<AccessControlDTO> Handle(GenericCommand<Guid, AccessControlDTO> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = await Repository
                .GetAll()
                .FirstOrDefaultAsync(c => c.Id.Equals(request.Input), cancellationToken: cancellationToken)!;

            return model.MapTo<AccessControlDTO>();
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateAccessControlRequest",
                "Erro ao obter acesso. Se o erro persistir Contate o Suporte do sistema!"));

            return default;
        }
    }

    public async Task<bool> Handle(GenericCommand<CreateInput<AccessControlDTO>, bool> request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.Input.Model.Connections.Any())
            {
                foreach (var accessControl in request.Input.Model.MapAccsessControl())
                {

                    if (!Repository.Any(c => c.Id == accessControl.Id))
                    {
                      await Repository.AddAsync(accessControl.MapTo<AccessControl>(), true, cancellationToken: cancellationToken);
                    }
                    else
                    {
                        await Repository.UpdateAsync(accessControl.MapTo<AccessControl>(), true, cancellationToken: cancellationToken);
                    }
                }
            }

            return true;
        }
        catch (Exception e)
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("CreateAccessControl",
                "Erro ao criar acessos. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<UpdateInput<AccessControlDTO>, bool> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = request.Input!.Model.MapTo<AccessControl>();
            ;
            await Repository.UpdateAsync(model, true, cancellationToken);

            return true;
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("UpdateAccessControl",
                "Erro ao atulizar acesso. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<DeleteInput<Guid, AccessControl>, bool> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var model = Repository.Find(request.Input.Id);

            if (model.IsNull())
            {
                await MediatorHandler!.PublishNotification(new DomainNotification("DeleteAccessControl",
                    $"Contato n??o encontrado!"));
                return false;
            }

            await Repository.DeleteAsync(model, true, cancellationToken);
            return true;
        }
        catch
        {
            await MediatorHandler!.PublishNotification(new DomainNotification("DeleteAccessControl",
                "Erro ao atulizar acesso. Se o erro persistir Contate o Suporte do sistema!"));

            return false;
        }
    }
}