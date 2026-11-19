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

namespace Sindvamb.Service.Handlers.PartnerUnits;

public class PartnerUnitCommandHandler :
    ICommandHandler<GenericCommand<List<PartnerUnitDTO>>, List<PartnerUnitDTO>>,
    ICommandHandler<GenericCommand<Guid, PartnerUnitDTO>, PartnerUnitDTO>,
    ICommandHandler<GenericCommand<CreateInput<PartnerUnitDTO>, bool>, bool>,
    ICommandHandler<GenericCommand<UpdateInput<PartnerUnitDTO>, bool>, bool>,
    ICommandHandler<GenericCommand<DeleteInput<Guid, PartnerUnit>, bool>, bool>
{
    private readonly IConfiguration _configuration;

    protected AppSettingsSection AppSettings { get; set; }

    public IMediatorHandler? MediatorHandler { get; set; }

    public IOptions<AppSettingsSection> Options { get; }

    public ISindvambRepository<PartnerUnit, Guid> Repository { get; set; }

    public ISindvambRepository<Partner, Guid> PartnerRepository { get; set; }

    public ISindvambRepository<Role, Guid> RoleRepository { get; set; }

    public ISindvambRepository<UserStatus, Guid> UserStatusRepository { get; set; }

    public ISindvambRepository<RegistrationRequest, Guid> RegistrationRequestRepository { get; set; }

    public IMapper Mapper { get; set; }
    public ISindvambUnitOfWork UnitOfWork { get; set; }

    public PartnerUnitCommandHandler(
        IMediatorHandler mediatorHandler,
        IOptions<AppSettingsSection> options,
        IConfiguration configuration,
        IMapper mapper,
        ISindvambRepository<PartnerUnit, Guid> repository,
        ISindvambRepository<RegistrationRequest, Guid> registrationRequestRepository,
        ISindvambRepository<Role, Guid> roleRepository,
        ISindvambRepository<UserStatus, Guid> userStatusRepository, ISindvambRepository<Partner, Guid> partnerRepository, ISindvambUnitOfWork unitOfWork)
    {
        MediatorHandler = mediatorHandler;
        _configuration = configuration;
        Mapper = mapper;
        Repository = repository;
        RegistrationRequestRepository = registrationRequestRepository;
        RoleRepository = roleRepository;
        AppSettings = options.Value;
        UserStatusRepository = userStatusRepository;
        PartnerRepository = partnerRepository;
        UnitOfWork = unitOfWork;
    }


    public async Task<List<PartnerUnitDTO>> Handle(GenericCommand<List<PartnerUnitDTO>> request, CancellationToken cancellationToken)
    {
        return await Repository
            .GetAll()
            .Include(p => p.Partner)
            .Select(p => p.MapTo<PartnerUnitDTO>())
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<PartnerUnitDTO> Handle(GenericCommand<Guid, PartnerUnitDTO> request, CancellationToken cancellationToken)
    {

        return await Repository
           .Where(p => p.Id.Equals(request.Input))
           .Include(p => p.Partner)
           .Select(p => p.MapTo<PartnerUnitDTO>())
           .FirstOrDefaultAsync(cancellationToken: cancellationToken);

    }

    public async Task<bool> Handle(GenericCommand<CreateInput<PartnerUnitDTO>, bool> request, CancellationToken cancellationToken)
    {
        try
        {
            
            var unit = request.Input.Model.MapTo<PartnerUnit>();
            unit.Partner = null;
            var result = await Repository.AddAsync(unit, true, cancellationToken);

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<UpdateInput<PartnerUnitDTO>, bool> request, CancellationToken cancellationToken)
    {
        try
        {
            var partner = await Repository
                .Where(p => p.Id == request.Input.Model.Id)
                .FirstOrDefaultAsync();

            partner = request.Input.Model.MapTo<PartnerUnit>();

            await Repository.UpdateAsync(partner, true, cancellationToken);
            return true;
        }
        catch
        {

            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<DeleteInput<Guid, PartnerUnit>, bool> request, CancellationToken cancellationToken)
    {
        try
        {
            var user = Repository.Find(request.Input.Id);

            if (user.IsNull())
            {
                await MediatorHandler.PublishNotification(new DomainNotification(nameof(RegisterCommand), $"Unidade não encontrada!"));
                return false;
            }

            Repository.Delete(user, true);
            return true;
        }
        catch
        {

            return false;
        }
    }

}
