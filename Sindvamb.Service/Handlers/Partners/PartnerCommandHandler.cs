#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Events.BuildingBlocks.Messages;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Service.Commands.GenericCommands;
using Sindvamb.Service.Commands.Users;
using Sindvamb.Service.Inputs;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;

#endregion

namespace Sindvamb.Service.Handlers.Partners;

public class PartnerCommandHandler :
    ICommandHandler<GenericCommand<List<PartnerDTO>>, List<PartnerDTO>>,
    ICommandHandler<GenericCommand<Guid, PartnerDTO>, PartnerDTO>,
    ICommandHandler<GenericCommand<CreateInput<PartnerDTO>, bool>, bool>,
    ICommandHandler<GenericCommand<UpdateInput<PartnerDTO>, bool>, bool>,
    ICommandHandler<GenericCommand<DeleteInput<Guid, Partner>, bool>, bool>
{
    private readonly IConfiguration _configuration;

    protected AppSettingsSection AppSettings { get; set; }

    public IMediatorHandler? MediatorHandler { get; set; }


    public IOptions<AppSettingsSection> Options { get; }

    public ISindvambRepository<Partner, Guid> Repository { get; set; }
    public ISindvambRepository<PartnerUnit, Guid> PartnerUnitRepository { get; set; }

    public ISindvambRepository<Role, Guid> RoleRepository { get; set; }
    public ISindvambRepository<UserStatus, Guid> UserStatusRepository { get; set; }

    public ISindvambRepository<RegistrationRequest, Guid> RegistrationRequestRepository { get; set; }

    public IMapper Mapper { get; set; }

    public PartnerCommandHandler(
        IMediatorHandler mediatorHandler,
        IOptions<AppSettingsSection> options,
        IConfiguration configuration,
        IMapper mapper,
        ISindvambRepository<Partner, Guid> repository,
        ISindvambRepository<RegistrationRequest, Guid> registrationRequestRepository,
        ISindvambRepository<Role, Guid> roleRepository,
        ISindvambRepository<UserStatus, Guid> userStatusRepository,
        ISindvambRepository<PartnerUnit, Guid> partnerUnitRepository)
    {
        MediatorHandler = mediatorHandler;
        _configuration = configuration;
        Mapper = mapper;
        Repository = repository;
        RegistrationRequestRepository = registrationRequestRepository;
        RoleRepository = roleRepository;
        AppSettings = options.Value;
        UserStatusRepository = userStatusRepository;
        PartnerUnitRepository = partnerUnitRepository;
    }

    public async Task<List<PartnerDTO>> Handle(GenericCommand<List<PartnerDTO>> request, CancellationToken cancellationToken)
    {
        var result = await Repository
            .GetAll()
            .Select(p => p.MapTo<PartnerDTO>())
            .ToListAsync();
        
        return result;
    }

    public async Task<PartnerDTO> Handle(GenericCommand<Guid, PartnerDTO> request, CancellationToken cancellationToken)
    {

        return await Repository
           .Where(p => p.Id.Equals(request.Input))
           .Select(p => p.MapTo<PartnerDTO>())
           .FirstOrDefaultAsync();

    }

    public async Task<bool> Handle(GenericCommand<CreateInput<PartnerDTO>, bool> request, CancellationToken cancellationToken)
    {

        await Repository.AddAsync(request.Input.Model.MapTo<Partner>(), true, cancellationToken);

        return true;
    }

    public async Task<bool> Handle(GenericCommand<UpdateInput<PartnerDTO>, bool> request, CancellationToken cancellationToken)
    {
        try
        {
            var partner = await Repository
                .Where(p => p.Id == request.Input.Model.Id)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            partner = request.Input.Model.MapTo<Partner>();

            await Repository.UpdateAsync(partner, true, cancellationToken);

            return true;
        }
        catch
        {

            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<DeleteInput<Guid, Partner>, bool> request, CancellationToken cancellationToken)
    {
        var user = Repository.Find(request.Input.Id);

        if (user.IsNull())
        {
            await MediatorHandler.PublishNotification(new DomainNotification(nameof(RegisterCommand), $"Parceiro não encontrado!"));
            return false;
        }

        if (PartnerUnitRepository.Any(u => u.PartnerId == request.Input.Id))
        {
            await MediatorHandler.PublishNotification(new DomainNotification(nameof(RegisterCommand), $"Não foi possível excluir, existe unidades vinculada a esse parceiro!"));
            return false;
        }

        Repository.Delete(user, true);
        return true;
    }
}
