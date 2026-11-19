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

namespace Sindvamb.Service.Handlers.ProductCategories;

public class ProductCategoryCommandHandler :
    ICommandHandler<GenericCommand<List<ProductCategoryDTO>>, List<ProductCategoryDTO>>,
    ICommandHandler<GenericCommand<Guid, ProductCategoryDTO>, ProductCategoryDTO>,
    ICommandHandler<GenericCommand<CreateInput<ProductCategoryDTO>, bool>, bool>,
    ICommandHandler<GenericCommand<UpdateInput<ProductCategoryDTO>, bool>, bool>,
    ICommandHandler<GenericCommand<DeleteInput<Guid, ProductCategory>, bool>, bool>
{
    private readonly IConfiguration _configuration;

    protected AppSettingsSection AppSettings { get; set; }

    public IMediatorHandler? MediatorHandler { get; set; }


    public IOptions<AppSettingsSection> Options { get; }

    public ISindvambRepository<ProductCategory, Guid> Repository { get; set; }

    public ISindvambRepository<Order, Guid> OrderRepository { get; set; }

    public IMapper Mapper { get; set; }

    public ProductCategoryCommandHandler(
        IMediatorHandler mediatorHandler,
        IOptions<AppSettingsSection> options,
        IConfiguration configuration,
        IMapper mapper,
        ISindvambRepository<ProductCategory, Guid> repository,
        ISindvambRepository<Order, Guid> orderRepository)
    {
        MediatorHandler = mediatorHandler;
        _configuration = configuration;
        Mapper = mapper;
        Repository = repository;
        OrderRepository = orderRepository;
        AppSettings = options.Value;
    }


    public async Task<List<ProductCategoryDTO>> Handle(GenericCommand<List<ProductCategoryDTO>> request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await Repository
                .GetAll()
                .Select(p => p.MapTo<ProductCategoryDTO>())
                .ToListAsync(cancellationToken: cancellationToken);

            return result;
        }
        catch
        {

            return default;
        }
    }

    public async Task<ProductCategoryDTO> Handle(GenericCommand<Guid, ProductCategoryDTO> request, CancellationToken cancellationToken)
    {
        return await Repository
           .Where(p => p.Id.Equals(request.Input))
           .Select(p => p.MapTo<ProductCategoryDTO>())
           .FirstOrDefaultAsync(cancellationToken: cancellationToken);

    }

    public async Task<bool> Handle(GenericCommand<CreateInput<ProductCategoryDTO>, bool> request, CancellationToken cancellationToken)
    {
        var unit = request.Input.Model.MapTo<ProductCategory>();
        await Repository.AddAsync(unit, true, cancellationToken);

        return true;
    }

    public async Task<bool> Handle(GenericCommand<UpdateInput<ProductCategoryDTO>, bool> request, CancellationToken cancellationToken)
    {
        try
        {
            var partner = await Repository
                .Where(p => p.Id == request.Input.Model.Id)
                .FirstOrDefaultAsync();

            partner = request.Input.Model.MapTo<ProductCategory>();

            await Repository.UpdateAsync(partner, true, cancellationToken);
            return true;
        }
        catch
        {

            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<DeleteInput<Guid, ProductCategory>, bool> request, CancellationToken cancellationToken)
    {
        try
        {
            var client = Repository.Find(request.Input.Id);

            if (client.IsNull())
            {
                await MediatorHandler.PublishNotification(new DomainNotification("Delete Tipo Cliente", $"Tipo de Cliente não encontrado!"));
                return false;
            }

            if (OrderRepository.Any(o => o.ProductCategoryId == client.Id))
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
