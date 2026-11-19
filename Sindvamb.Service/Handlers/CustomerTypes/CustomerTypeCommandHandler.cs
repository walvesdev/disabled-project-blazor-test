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

namespace Sindvamb.Service.Handlers.CustomerTypes;

public class CustomerTypeCommandHandler :
    ICommandHandler<GenericCommand<List<CustomerTypeDTO>>, List<CustomerTypeDTO>>,
    ICommandHandler<GenericCommand<Guid, CustomerTypeDTO>, CustomerTypeDTO>,
    ICommandHandler<GenericCommand<CreateInput<CustomerTypeDTO>, bool>, bool>,
    ICommandHandler<GenericCommand<UpdateInput<CustomerTypeDTO>, bool>, bool>,
    ICommandHandler<GenericCommand<DeleteInput<Guid, CustomerType>, bool>, bool>
{
    private readonly IConfiguration _configuration;

    protected AppSettingsSection AppSettings { get; set; }

    public IMediatorHandler? MediatorHandler { get; set; }


    public IOptions<AppSettingsSection> Options { get; }

    public ISindvambRepository<CustomerType, Guid> Repository { get; set; }

    public ISindvambRepository<Customer, Guid> CustomerRepository { get; set; }

    public IMapper Mapper { get; set; }

    public CustomerTypeCommandHandler(
        IMediatorHandler mediatorHandler,
        IOptions<AppSettingsSection> options,
        IConfiguration configuration,
        IMapper mapper,
        ISindvambRepository<CustomerType, Guid> repository,
        ISindvambRepository<Customer, Guid> customerRepository)
    {
        MediatorHandler = mediatorHandler;
        _configuration = configuration;
        Mapper = mapper;
        Repository = repository;
        CustomerRepository = customerRepository;
        AppSettings = options.Value;
    }


    public async Task<List<CustomerTypeDTO>> Handle(GenericCommand<List<CustomerTypeDTO>> request, CancellationToken cancellationToken)
    {
        return await Repository
            .GetAll()
            .Select(p => p.MapTo<CustomerTypeDTO>())
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<CustomerTypeDTO> Handle(GenericCommand<Guid, CustomerTypeDTO> request, CancellationToken cancellationToken)
    {

        return await Repository
           .Where(p => p.Id.Equals(request.Input))
           .Select(p => p.MapTo<CustomerTypeDTO>())
           .FirstOrDefaultAsync(cancellationToken: cancellationToken);

    }

    public async Task<bool> Handle(GenericCommand<CreateInput<CustomerTypeDTO>, bool> request, CancellationToken cancellationToken)
    {
        try
        {
            var unit = request.Input.Model.MapTo<CustomerType>();
            await Repository.AddAsync(unit, true, cancellationToken);

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<UpdateInput<CustomerTypeDTO>, bool> request, CancellationToken cancellationToken)
    {
        try
        {
            var partner = await Repository
                .Where(p => p.Id == request.Input.Model.Id)
                .FirstOrDefaultAsync();

            partner = request.Input.Model.MapTo<CustomerType>();

            await Repository.UpdateAsync(partner, true, cancellationToken);
            return true;
        }
        catch
        {

            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<DeleteInput<Guid, CustomerType>, bool> request, CancellationToken cancellationToken)
    {
        try
        {
            var client = Repository.Find(request.Input.Id);

            if (client.IsNull())
            {
                await MediatorHandler.PublishNotification(new DomainNotification("Delete Tipo Cliente", $"Tipo de Cliente não encontrado!"));
                return false;
            }

            if (CustomerRepository.Any(o => o.CustomerTypeId == client.Id))
            {
                await MediatorHandler.PublishNotification(new DomainNotification("Delete", $"Esse item não pode ser deletado, por que está sendo utilizado por alguns Clientes!"));
                return false;
            }

            Repository.Delete(client, true);
            return true;
        }
        catch (Exception e)
        {

            return false;
        }
    }
}
