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
using Sindvamb.Service.Inputs;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;

#endregion

namespace Sindvamb.Service.Handlers.SpecialNeed;

public class SpecialNeedsCommandHandler :
    ICommandHandler<GenericCommand<List<SpecialNeedsDTO>>, List<SpecialNeedsDTO>>,
    ICommandHandler<GenericCommand<Guid, SpecialNeedsDTO>, SpecialNeedsDTO>,
    ICommandHandler<GenericCommand<CreateInput<SpecialNeedsDTO>, bool>, bool>,
    ICommandHandler<GenericCommand<UpdateInput<SpecialNeedsDTO>, bool>, bool>,
    ICommandHandler<GenericCommand<DeleteInput<Guid, SpecialNeeds>, bool>, bool>
{
    private readonly IConfiguration _configuration;

    protected AppSettingsSection AppSettings { get; set; }

    public IMediatorHandler? MediatorHandler { get; set; }


    public IOptions<AppSettingsSection> Options { get; }

    public ISindvambRepository<SpecialNeeds, Guid> Repository { get; set; }

    public ISindvambRepository<Order, Guid> OrderRepository { get; set; }

    public IMapper Mapper { get; set; }

    public SpecialNeedsCommandHandler(
        IMediatorHandler mediatorHandler,
        IOptions<AppSettingsSection> options,
        IConfiguration configuration,
        IMapper mapper,
        ISindvambRepository<SpecialNeeds, Guid> repository,
        ISindvambRepository<Order, Guid> orderRepository)
    {
        MediatorHandler = mediatorHandler;
        _configuration = configuration;
        Mapper = mapper;
        Repository = repository;
        OrderRepository = orderRepository;
        AppSettings = options.Value;
    }


    public async Task<List<SpecialNeedsDTO>> Handle(GenericCommand<List<SpecialNeedsDTO>> request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await Repository
                .GetAll()
                .Select(p => p.MapTo<SpecialNeedsDTO>())
                .ToListAsync(cancellationToken: cancellationToken);

            return result;
        }
        catch (Exception e)
        {
            await MediatorHandler.PublishNotification(new DomainNotification("List<SpecialNeedsDTO>", $"Erro ao obter necessidads especiais!"));
            return null;
        }
    }

    public async Task<SpecialNeedsDTO> Handle(GenericCommand<Guid, SpecialNeedsDTO> request, CancellationToken cancellationToken)
    {

        return await Repository
           .Where(p => p.Id.Equals(request.Input))
           .Select(p => p.MapTo<SpecialNeedsDTO>())
           .FirstOrDefaultAsync(cancellationToken: cancellationToken);

    }

    public async Task<bool> Handle(GenericCommand<CreateInput<SpecialNeedsDTO>, bool> request, CancellationToken cancellationToken)
    {
        var unit = request.Input.Model.MapTo<SpecialNeeds>();
        await Repository.AddAsync(unit, true, cancellationToken);

        return true;
    }

    public async Task<bool> Handle(GenericCommand<UpdateInput<SpecialNeedsDTO>, bool> request, CancellationToken cancellationToken)
    {
        try
        {
            var partner = await Repository
                .Where(p => p.Id == request.Input.Model.Id)
                .FirstOrDefaultAsync();

            partner = request.Input.Model.MapTo<SpecialNeeds>();

            await Repository.UpdateAsync(partner, true, cancellationToken);
            return true;
        }
        catch
        {

            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<DeleteInput<Guid, SpecialNeeds>, bool> request, CancellationToken cancellationToken)
    {
        try
        {
            var client = Repository.Find(request.Input.Id);

            if (client.IsNull())
            {
                await MediatorHandler.PublishNotification(new DomainNotification("Delete", $"SpecialNeeds não encontrado!"));
                return false;
            }

            if (OrderRepository.Any(o => o.Id == client.Id))
            {
                await MediatorHandler.PublishNotification(new DomainNotification("Delete", $"Esse item não pode ser deletado, por que está sendo utilizado por algumas Demandas!"));
                return false;
            }

            Repository.Delete(client, true);
            return true;
        }
        catch
        {

            return false;
        }
    }
}
