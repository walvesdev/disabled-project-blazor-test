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
using Sindvamb.Service.Commands.Orders;
using Sindvamb.Service.Inputs;
using Sindvamb.Service.Net;
using Sindvamb.Templates;
using Sindvamb.Templates.Pages.Demandas;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

#endregion

namespace Sindvamb.Service.Handlers.Orders;

public class OrderCommandHandler :
    ICommandHandler<GenericCommand<List<OrderDTO>>, List<OrderDTO>>,
    ICommandHandler<GenericCommand<Guid, OrderDTO>, OrderDTO>,
    ICommandHandler<GenericCommand<CreateInput<OrderDTO>, bool>, bool>,
    ICommandHandler<GenericCommand<UpdateInput<OrderDTO>, bool>, bool>,
    ICommandHandler<UpdateStatusIndexCommand, bool>,
    ICommandHandler<GenericCommand<DeleteInput<Guid, Order>, bool>, bool>

{
    private readonly IConfiguration _configuration;

    protected AppSettingsSection AppSettings { get; set; }

    public IMediatorHandler? MediatorHandler { get; set; }


    public IOptions<AppSettingsSection> Options { get; }

    private ISmtpClientHelper Email { get; set; }

    public ISindvambRepository<Order, Guid> Repository { get; set; }
    public ISindvambRepository<OrderStatus, Guid> OrderStatusRepository { get; set; }
    public ISindvambRepository<OrderTracking, Guid> OrderTrackingRepository { get; set; }
    public ISindvambRepository<OrderEmail, Guid> OrderEmailRepository { get; set; }

    public ISindvambRepository<CustomerOrder, Guid> CustomerOrderRepository { get; set; }

    public ISindvambRepository<Customer, Guid> CustomeRepository { get; set; }

    public IMapper Mapper { get; set; }

    public OrderCommandHandler(
        IMediatorHandler mediatorHandler,
        IOptions<AppSettingsSection> options,
        IConfiguration configuration,
        IMapper mapper,
        ISindvambRepository<Order, Guid> repository,
        ISindvambRepository<CustomerOrder, Guid> customerOrderRepository,
        ISindvambRepository<Customer, Guid> customeRepository,
        ISindvambRepository<OrderEmail, Guid> orderEmailRepository,
        ISindvambRepository<OrderTracking, Guid> orderTrackingRepository,
        ISmtpClientHelper email, ISindvambRepository<OrderStatus, Guid> orderStatusRepository)
    {
        MediatorHandler = mediatorHandler;
        _configuration = configuration;
        Mapper = mapper;
        Repository = repository;
        CustomerOrderRepository = customerOrderRepository;
        CustomeRepository = customeRepository;
        OrderEmailRepository = orderEmailRepository;
        OrderTrackingRepository = orderTrackingRepository;
        Email = email;
        OrderStatusRepository = orderStatusRepository;
        AppSettings = options.Value;
    }


    public async Task<List<OrderDTO>> Handle(GenericCommand<List<OrderDTO>> request, CancellationToken cancellationToken)
    {
        return await Repository
            .GetAll()
            .Include(o => o.ProductArea)
            .Include(o => o.OrderStatus)
            .Include(o => o.PartnerUnit)
            .Include(o => o.CustomerOrder)
            .ThenInclude(o => o.Customer)
            .ThenInclude(o => o.User)
            .Select(p => p.MapTo<OrderDTO>())
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<OrderDTO> Handle(GenericCommand<Guid, OrderDTO> request, CancellationToken cancellationToken)
    {
        return await Repository
            .Where(p => p.Id.Equals(request.Input))
            .Include(o => o.ProductArea)
            .Include(o => o.CustomerOrder)
            .ThenInclude(oc => oc.Customer)
            .ThenInclude(ocu => ocu.User)
            .Include(o => o.ProductCategory)
            .Include(o => o.OrderStatus)
            .Include(o => o.OrderTracking)
            .Select(p => MapDTO(p))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
    }
    
    private static OrderDTO MapDTO(Order order)
    {
        var dto = order.MapTo<OrderDTO>();

        try
        {
            dto.ClientId = (Guid)order.CustomerOrder.FirstOrDefault(c => (bool)c.IsClient)?.CustomerId;
            //dto.SindvambId = (Guid)order.CustomerOrder.FirstOrDefault(c => (bool)c.IsSindvamb)?.CustomerId;
            dto.DirectorId = (Guid)order.CustomerOrder.FirstOrDefault(c => (bool)c.IsDirector)?.CustomerId;
            dto.ManagerId = (Guid)order.CustomerOrder.FirstOrDefault(c => (bool)c.IsManager)?.CustomerId;
            //dto.PresidencyId = (Guid)order.CustomerOrder.FirstOrDefault(c => (bool)c.IsPresidency)?.CustomerId;
        }
        catch
        {
        }

        return dto;
    }

    public async Task<bool> Handle(GenericCommand<CreateInput<OrderDTO>, bool> request, CancellationToken cancellationToken)
    {
        try
        {
            var order = new Order();
            order = request.Input.Model.MapTo<Order>();
            order.Id = SequentialGuidGenerator.NewSequentialGuid();
            order.Protocol = $"{order.Id.ToString()[0..8].ToUpper().Replace("-", "").PadLeft(8, '0')}{DateTime.UtcNow:yyyy}";
            order.Enabled = true;
            order.Sigla = request.Input.Model.Sigla;

            if (request.Input.Model.SendEmailSindvamb)
            {
                var bpEmail = await CustomeRepository
                    .Where(c => c.Id == request.Input.Model.SindvambId)
                    .Include(c => c.User)
                    .Select(c => c.User.Email)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (!bpEmail.IsNullOrWhiteSpace())
                {
                    order.OrderEmails.Add(new OrderEmail
                    {
                        Email = bpEmail
                    });
                }
            }

            if (request.Input.Model.SendEmailOrderCreator)
            {
                var email = AppServices.GetUserEmail();
                if (!email.IsNullOrWhiteSpace())
                {
                    order.OrderEmails.Add(new OrderEmail
                    {
                        Email = email
                    });
                }
            }

            if (!request.Input.Model.EmailWithCopy.IsNullOrWhiteSpace())
            {
                var list = new List<string>();

                if (request.Input.Model.EmailWithCopy.Contains(";"))
                {
                    list = request.Input.Model.EmailWithCopy.Split(";").ToList();
                }
                else
                {
                    list.Add(request.Input.Model.EmailWithCopy);
                }

                list.ForEach(item =>
                {
                    order.OrderEmails.Add(new OrderEmail
                    {
                        Email = item
                    });
                });
            }

            //order.CustomerOrder.Add(new CustomerOrder
            //{
            //    IsClient = false,
            //    IsPresidency = false,
            //    IsSindvamb = true,
            //    CustomerId = request.Input.Model.SindvambId,
            //    OrderId = order.Id
            //});

            order.CustomerOrder.Add(new CustomerOrder
            {
                IsClient = false,
                IsPresidency = false,
                IsSindvamb = false,
                IsManager = false,
                IsDirector = true,
                CustomerId = request.Input.Model.DirectorId,
                OrderId = order.Id
            });

            order.CustomerOrder.Add(new CustomerOrder
            {
                IsClient = false,
                IsPresidency = false,
                IsSindvamb = false,
                IsManager = true,
                IsDirector = false,
                CustomerId = request.Input.Model.ManagerId,
                OrderId = order.Id
            });

            order.CustomerOrder.Add(new CustomerOrder
            {
                IsClient = true,
                IsPresidency = false,
                IsSindvamb = false,
                CustomerId = request.Input.Model.ClientId,
                OrderId = order.Id
            });

            order.OrderTracking.Add(new OrderTracking
            {
                Id = SequentialGuidGenerator.NewSequentialGuid(),
                OrderId = order.Id,
                History = $"USUÁRIO MATRICULA: {AppServices.GetUserMatricula()} CRIOU A DEMANDA.",
                TrackDate = DateTime.UtcNow
            });

            //TODO Configurar Ticket Para os responsáveis pela demanda


            await Repository.AddAsync(order, true, cancellationToken);

            if (request.Input.Model.SendEmailSindvamb)
            {
                var orderDb = Repository
                    .GetAll(o => o.Id == order.Id)
                    .Include(o => o.PartnerUnit)
                    .Include(o => o.CustomerOrder)
                    .ThenInclude(c => c.Customer)
                    .ThenInclude(c => c.User)
                    .Include(o => o.CustomerOrder)
                    .Include(o => o.ProductArea)
                    .Include(o => o.OrderType)
                    .Include(o => o.ProductCategory)
                    .FirstOrDefault();

                var model = new OrderCreate
                {
                    OrderId = orderDb.Id.ToString(),
                    OrderProtocol = orderDb.Protocol,
                    BusinnesPartnerName = orderDb.CustomerOrder.FirstOrDefault(c => (bool)c.IsSindvamb).Customer.User.Name,
                    PartnerUnitName = orderDb.PartnerUnit.Name,
                    ProductAreaName = orderDb.ProductArea.Description,
                    ClientName = orderDb.CustomerOrder.FirstOrDefault(c => (bool)c.IsClient).Customer.User.Name,
                    ClientEmail = orderDb.CustomerOrder.FirstOrDefault(c => (bool)c.IsClient).Customer.User.Email,
                    OrderTypeName = orderDb.OrderType.Description,
                    OrderDescription = orderDb.Description,
                    ProductCategoryName = orderDb.ProductCategory.Description,
                    FrontEndUrl = AppSettings.FrontEndUrl
                };

                var result = await Email.SendSmtpAsync(
                    $"Nova Demanda {orderDb.Protocol} - Sindvamb",
                    orderDb.CustomerOrder.FirstOrDefault(c => (bool)c.IsSindvamb).Customer.User.Email,
                    TemplateKey.ORDERCREATE,
                    model
                );
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task<bool> Handle(GenericCommand<UpdateInput<OrderDTO>, bool> request, CancellationToken cancellationToken)
    {
        try
        {
            CustomerOrderRepository.Delete(c => c.OrderId == request.Input.Model.Id, true);
            OrderEmailRepository.Delete(e => e.OrderId == request.Input.Model.Id, true);

            var order = request.Input.Model.MapTo<Order>();
            order.CustomerOrder = null;
            order.OrderEmails = null;
            order.OrderTracking = null;
            order.OrderStatus = null;
            order.ProductArea = null;
            order.ProductCategory = null;
            order.Sigla = request.Input.Model.Sigla;


            order.Enabled = true;
            var orderEmails = new List<OrderEmail>();

            if (request.Input.Model.SendEmailSindvamb)
            {
                var bpEmail = await CustomeRepository
                    .Where(c => c.Id == request.Input.Model.SindvambId)
                    .Include(c => c.User)
                    .Select(c => c.User.Email)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

                if (!bpEmail.IsNullOrWhiteSpace())
                {
                    orderEmails.Add(new OrderEmail
                    {
                        Email = bpEmail,
                        OrderId = order.Id
                    });
                }
            }

            if (request.Input.Model.SendEmailOrderCreator)
            {
                var email = AppServices.GetUserEmail();
                if (!email.IsNullOrWhiteSpace())
                {
                    orderEmails.Add(new OrderEmail
                    {
                        Email = email,
                        OrderId = order.Id
                    });
                }
            }

            if (!request.Input.Model.EmailWithCopy.IsNullOrWhiteSpace())
            {
                var list = new List<string>();

                if (request.Input.Model.EmailWithCopy.Contains(";"))
                {
                    list = request.Input.Model.EmailWithCopy.Split(";").ToList();
                }
                else
                {
                    list.Add(request.Input.Model.EmailWithCopy);
                }

                list.ForEach(item =>
                {
                    orderEmails.Add(new OrderEmail
                    {
                        Email = item,
                        OrderId = order.Id
                    });
                });
            }

            OrderEmailRepository.Context.AddRange(orderEmails);
            OrderEmailRepository.Context.SaveChanges(true);

            var listCo = new List<CustomerOrder>()
            {
                //new CustomerOrder
                //{
                //    IsClient = false,
                //    IsPresidency = false,
                //    IsSindvamb = true,
                //    CustomerId = request.Input.Model.SindvambId,
                //    OrderId = order.Id
                //},
                new CustomerOrder
                {
                    IsClient = true,
                    IsPresidency = false,
                    IsSindvamb = false,
                    CustomerId = request.Input.Model.ClientId,
                    OrderId = order.Id
                },
                new  CustomerOrder
                {
                    IsClient = false,
                    IsPresidency = false,
                    IsSindvamb = false,
                    IsManager = false,
                    IsDirector = true,
                    CustomerId = request.Input.Model.DirectorId,
                    OrderId = order.Id
                },
                    new CustomerOrder
                {
                    IsClient = false,
                    IsPresidency = false,
                    IsSindvamb = false,
                    IsManager = true,
                    IsDirector = false,
                    CustomerId = request.Input.Model.ManagerId,
                    OrderId = order.Id
                }
            };

            CustomerOrderRepository.Context.AddRange(listCo);
            CustomerOrderRepository.Context.SaveChanges(true);

            OrderTrackingRepository.Add(new OrderTracking
            {
                Id = SequentialGuidGenerator.NewSequentialGuid(),
                OrderId = order.Id,
                History = $"USUÁRIO MATRICULA: {AppServices.GetUserMatricula()} ATUALIZOU A DEMANDA.",
                TrackDate = DateTime.UtcNow
            });
            OrderTrackingRepository.SaveChanges();

            //TODO Configurar Ticket Para os responsáveis pela demanda

            await Repository.UpdateAsync(order, true, cancellationToken);

            if (request.Input.Model.SendEmailSindvamb)
            {
                var orderDb = Repository
                    .GetAll(o => o.Id == order.Id)
                    .Include(o => o.PartnerUnit)
                    .Include(o => o.CustomerOrder)
                    .ThenInclude(c => c.Customer)
                    .ThenInclude(c => c.User)
                    .Include(o => o.CustomerOrder)
                    .Include(o => o.ProductArea)
                    .Include(o => o.OrderType)
                    .Include(o => o.ProductCategory)
                    .FirstOrDefault();

                var model = new OrderCreate
                {
                    OrderId = orderDb.Id.ToString(),
                    OrderProtocol = orderDb.Protocol,
                    BusinnesPartnerName = orderDb.CustomerOrder.FirstOrDefault(c => (bool)c.IsSindvamb).Customer.User.Name,
                    PartnerUnitName = orderDb.PartnerUnit.Name,
                    ProductAreaName = orderDb.ProductArea.Description,
                    ClientName = orderDb.CustomerOrder.FirstOrDefault(c => (bool)c.IsClient).Customer.User.Name,
                    ClientEmail = orderDb.CustomerOrder.FirstOrDefault(c => (bool)c.IsClient).Customer.User.Email,
                    OrderTypeName = orderDb.OrderType.Description,
                    OrderDescription = orderDb.Description,
                    ProductCategoryName = orderDb.ProductCategory.Description,
                    FrontEndUrl = AppSettings.FrontEndUrl
                };

                var result = await Email.SendSmtpAsync(
                    $"Nova Demanda {orderDb.Protocol} - Sindvamb",
                    orderDb.CustomerOrder.FirstOrDefault(c => (bool)c.IsSindvamb).Customer.User.Email,
                    TemplateKey.ORDERCREATE,
                    model
                );
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Handle(GenericCommand<DeleteInput<Guid, Order>, bool> request, CancellationToken cancellationToken)
    {
        try
        {
            var client = Repository.Find(request.Input.Id);

            if (client.IsNull())
            {
                await MediatorHandler.PublishNotification(new DomainNotification("Delete Demanda", $"Tipo de Demanda não encontrado!"));
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

    public async Task<bool> Handle(UpdateStatusIndexCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var order = request.Input.MapTo<Order>(); 
            order.CustomerOrder = null;
            order.OrderEmails = null;
            order.OrderTracking = null;
            order.ProductArea = null;
            order.ProductCategory = null;

            order.OrderStatusId = OrderStatusRepository.FirstOrDefault(o => o.Code.Equals(order.OrderStatus.Code)).Id;
            order.OrderStatus = null;

            await Repository.UpdateAsync(order, true, cancellationToken);

            return true;
        }
        catch
        {
            return false;
        }
    }
}
