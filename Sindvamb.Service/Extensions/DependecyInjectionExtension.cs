#region

using System;
using System.Collections.Generic;
using Castle.Core.Resource;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sindvamb.Domain.DTO;
using Sindvamb.Domain.Entities;
using Sindvamb.Service.Commands.Customers;
using Sindvamb.Service.Commands.FileControls;
using Sindvamb.Service.Commands.GenericCommands;
using Sindvamb.Service.Commands.Users;
using Sindvamb.Service.Handlers.AccessControls;
using Sindvamb.Service.Handlers.ContactRequestsRequest;
using Sindvamb.Service.Handlers.Contacts;
using Sindvamb.Service.Handlers.Customers;
using Sindvamb.Service.Handlers.CustomerTypes;
using Sindvamb.Service.Handlers.EducationDegrees;
using Sindvamb.Service.Handlers.FileControls;
using Sindvamb.Service.Handlers.Orders;
using Sindvamb.Service.Handlers.OrdersStatus;
using Sindvamb.Service.Handlers.OrderTypes;
using Sindvamb.Service.Handlers.Partners;
using Sindvamb.Service.Handlers.PartnerUnits;
using Sindvamb.Service.Handlers.ProductAreas;
using Sindvamb.Service.Handlers.ProductCategories;
using Sindvamb.Service.Handlers.SpecialNeed;
using Sindvamb.Service.Handlers.Users;
using Sindvamb.Service.Inputs;

#endregion

namespace Sindvamb.Service.Extensions;

public static class DependecyInjectionExtension
{
    public static IServiceCollection AddServicesModule(this IServiceCollection services,
     IConfiguration configuration)
    {
        // ====================================
        // Commands
        // ====================================

        //AccessControl
        services.AddScoped<IRequestHandler<GenericCommand<List<AccessControlDTO>>, List<AccessControlDTO>>, AccessControlCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<Guid, AccessControlDTO>, AccessControlDTO>, AccessControlCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<AccessControlDTO>, bool>, bool>, AccessControlCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<UpdateInput<AccessControlDTO>, bool>, bool>, AccessControlCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<DeleteInput<Guid, AccessControl>, bool>, bool>, AccessControlCommandHandler>();

        //FileControl
        services.AddScoped<IRequestHandler<GenericCommand<List<FileControlDTO>>, List<FileControlDTO>>, FileControlCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<Guid, FileControlDTO>, FileControlDTO>, FileControlCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<FileControlDTO>, bool>, bool>, FileControlCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<UpdateInput<FileControlDTO>, bool>, bool>, FileControlCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<DeleteInput<Guid, FileControl>, bool>, bool>, FileControlCommandHandler>();
        services.AddScoped<IRequestHandler<ApprovedChangeCommand, bool>, FileControlCommandHandler>();

        // Partner

        services.AddScoped<IRequestHandler<GenericCommand<List<PartnerDTO>>, List<PartnerDTO>>, PartnerCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<Guid, PartnerDTO>, PartnerDTO>, PartnerCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<PartnerDTO>, bool>, bool>, PartnerCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<UpdateInput<PartnerDTO>, bool>, bool>, PartnerCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<DeleteInput<Guid, Partner>, bool>, bool>, PartnerCommandHandler>();


        // PartnerUNit

        services.AddScoped<IRequestHandler<GenericCommand<List<PartnerUnitDTO>>, List<PartnerUnitDTO>>, PartnerUnitCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<Guid, PartnerUnitDTO>, PartnerUnitDTO>, PartnerUnitCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<PartnerUnitDTO>, bool>, bool>, PartnerUnitCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<UpdateInput<PartnerUnitDTO>, bool>, bool>, PartnerUnitCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<DeleteInput<Guid, PartnerUnit>, bool>, bool>, PartnerUnitCommandHandler>();

        //Users

        services.AddScoped<IRequestHandler<GenericCommand<List<UserStatusDTO>>, List<UserStatusDTO>>, UserCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<UserDTO>, bool>, bool>, UserCommandHandler>();
        //services.AddScoped<IRequestHandler<RequestLinkUpdatePasswordCommand, bool>, UserCommandHandler>();

        //Contact
        services.AddScoped<IRequestHandler<GenericCommand<List<ContactDTO>>, List<ContactDTO>>, ContactCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<Guid, ContactDTO>, ContactDTO>, ContactCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<ContactDTO>, bool>, bool>, ContactCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<UpdateInput<ContactDTO>, bool>,bool>, ContactCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<DeleteInput<Guid, Contact>, bool>, bool>, ContactCommandHandler>();
        
        //ContactRequest
        services.AddScoped<IRequestHandler<GenericCommand<List<ContactRequestDTO>>, List<ContactRequestDTO>>, ContactRequestCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<Guid, ContactRequestDTO>, ContactRequestDTO>, ContactRequestCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<ContactRequestDTO>, bool>, bool>, ContactRequestCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<UpdateInput<ContactRequestDTO>, bool>,bool>, ContactRequestCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<DeleteInput<Guid, ContactRequest>, bool>, bool>, ContactRequestCommandHandler>();

        //Roles

        services.AddScoped<IRequestHandler<GenericCommand<List<RoleDTO>>, List<RoleDTO>>, UserCommandHandler>();


        // Customer

        services.AddScoped<IRequestHandler<GenericCommand<List<CustomerDTO>>, List<CustomerDTO>>, CustomerCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<Guid, CustomerDTO>, CustomerDTO>, CustomerCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<CustomerDTO>, CustomerDTO>, CustomerDTO>, CustomerCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<UpdateInput<CustomerDTO>, bool>, bool>, CustomerCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<DeleteInput<Guid, Customer>, bool>, bool>, CustomerCommandHandler>();
        services.AddScoped<IRequestHandler<GetFullCustomerComand, Customer>, CustomerCommandHandler>();
        services.AddScoped<IRequestHandler<CreateDependentComnand, bool>, CustomerCommandHandler>();


        // CustomerType

        services.AddScoped<IRequestHandler<GenericCommand<List<CustomerTypeDTO>>, List<CustomerTypeDTO>>, CustomerTypeCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<Guid, CustomerTypeDTO>, CustomerTypeDTO>, CustomerTypeCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<CustomerTypeDTO>, bool>, bool>, CustomerTypeCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<UpdateInput<CustomerTypeDTO>, bool>, bool>, CustomerTypeCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<DeleteInput<Guid, CustomerType>, bool>, bool>, CustomerTypeCommandHandler>();


        // ProductArea

        services.AddScoped<IRequestHandler<GenericCommand<List<ProductAreaDTO>>, List<ProductAreaDTO>>, ProductAreaCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<Guid, ProductAreaDTO>, ProductAreaDTO>, ProductAreaCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<ProductAreaDTO>, bool>, bool>, ProductAreaCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<UpdateInput<ProductAreaDTO>, bool>, bool>, ProductAreaCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<DeleteInput<Guid, ProductArea>, bool>, bool>, ProductAreaCommandHandler>();


        // OrderType

        services.AddScoped<IRequestHandler<GenericCommand<List<OrderTypeDTO>>, List<OrderTypeDTO>>, OrderTypeCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<Guid, OrderTypeDTO>, OrderTypeDTO>, OrderTypeCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<OrderTypeDTO>, bool>, bool>, OrderTypeCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<UpdateInput<OrderTypeDTO>, bool>, bool>, OrderTypeCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<DeleteInput<Guid, OrderType>, bool>, bool>, OrderTypeCommandHandler>();


        // SpecialNeeds

        services.AddScoped<IRequestHandler<GenericCommand<List<SpecialNeedsDTO>>, List<SpecialNeedsDTO>>, SpecialNeedsCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<Guid, SpecialNeedsDTO>, SpecialNeedsDTO>, SpecialNeedsCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<SpecialNeedsDTO>, bool>, bool>, SpecialNeedsCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<UpdateInput<SpecialNeedsDTO>, bool>, bool>, SpecialNeedsCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<DeleteInput<Guid, SpecialNeeds>, bool>, bool>, SpecialNeedsCommandHandler>();

        // EducationDegree

        services.AddScoped<IRequestHandler<GenericCommand<List<EducationDegreeDTO>>, List<EducationDegreeDTO>>, EducationDegreeCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<Guid, EducationDegreeDTO>, EducationDegreeDTO>, EducationDegreeCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<EducationDegreeDTO>, bool>, bool>, EducationDegreeCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<UpdateInput<EducationDegreeDTO>, bool>, bool>, EducationDegreeCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<DeleteInput<Guid, EducationDegree>, bool>, bool>, EducationDegreeCommandHandler>();

        // ProductCategory

        services.AddScoped<IRequestHandler<GenericCommand<List<ProductCategoryDTO>>, List<ProductCategoryDTO>>, ProductCategoryCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<Guid, ProductCategoryDTO>, ProductCategoryDTO>, ProductCategoryCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<ProductCategoryDTO>, bool>, bool>, ProductCategoryCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<UpdateInput<ProductCategoryDTO>, bool>, bool>, ProductCategoryCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<DeleteInput<Guid, ProductCategory>, bool>, bool>, ProductCategoryCommandHandler>();

        // OrderStatus

        services.AddScoped<IRequestHandler<GenericCommand<List<OrderStatusDTO>>, List<OrderStatusDTO>>, OrderStatusCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<Guid, OrderStatusDTO>, OrderStatusDTO>, OrderStatusCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<OrderStatusDTO>, bool>, bool>, OrderStatusCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<UpdateInput<OrderStatusDTO>, bool>, bool>, OrderStatusCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<DeleteInput<Guid, OrderStatus>, bool>, bool>, OrderStatusCommandHandler>();

        // Order

        services.AddScoped<IRequestHandler<GenericCommand<List<OrderDTO>>, List<OrderDTO>>, OrderCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<Guid, OrderDTO>, OrderDTO>, OrderCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<CreateInput<OrderDTO>, bool>, bool>, OrderCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<UpdateInput<OrderDTO>, bool>, bool>, OrderCommandHandler>();
        services.AddScoped<IRequestHandler<GenericCommand<DeleteInput<Guid, Order>, bool>, bool>, OrderCommandHandler>();

        // ====================================
        // Queries
        // ====================================

        //services.AddScoped<IQueryHandler<GetAllAccountQuery, GetAllAccountResponse>, AccountQueryHandler>();

        return services;
    }
}