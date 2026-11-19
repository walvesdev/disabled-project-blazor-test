#region

using System.Reflection;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sindvamb.Caching;
using Sindvamb.Domain.Entities;
using Sindvamb.Events.BuildingBlocks.Handlers;
using Sindvamb.Files.Pdf;
using Sindvamb.Files.QrCode;
using Sindvamb.Repository;
using Sindvamb.Repository.Contexts;
using Sindvamb.Repository.Interfaces;
using Sindvamb.Repository.ORM;
using Sindvamb.Repository.Repositories;
using Sindvamb.Repository.UnitsOfWork;
using Sindvamb.Security.Services;
using Sindvamb.Service.Extensions;
using Sindvamb.Service.Net;
using Microsoft.AspNetCore.Http;
using QRCoder;
using Sindvamb.Utils;
using Sindvamb.Events.BuildingBlocks.Mediator;
using Sindvamb.Utils.Services.Http;
using Sindvamb.Utils.Types;
using Sindvamb.Utils.Services.DependencyInjection;
using Sindvamb.Utils.Extension;
using Sindvamb.Utils.Helpers;
using Sindvamb.Events.BuildingBlocks.EventSourcing;
using MediatR;
using Sindvamb.Events.BuildingBlocks.CommonMessages.Notifications;
using Spire.Pdf;

#endregion

namespace Sindvamb.Application.Core.Extensions;

public static class DependecyInjectionExtension
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services,
        IConfiguration configuration, Assembly? assemblie = null,
        AppSettingsSection appSettings = null)
    {
        services.AddOptions();
        services.Configure<AppSettingsSection>(configuration.GetSection("AppSettings"));

        services.AddScoped<LoginService>();
        services.AddScoped<User>();

        services.AddScoped<MediatorHangfireBridge>();
        services.AddScoped(typeof(HttpClientService<,,>));

        services.AddTransient<ICancellationTokenProvider, CancellationTokenProvider>();
        services.AddTransient<ILazyServiceProvider, LazyServiceProvider>();

        services.AddScoped<QRCodeGenerator>();
        services.AddScoped<QRCodeFactory>();
        services.AddScoped<PdfGenerator>();
        services.AddScoped<PdfDocument>();

        // services.AddScoped<ISmtpClientHelper>(imp => new SmtpClientHelper(appSettings));
        services.AddScoped<ISmtpClientHelper, SmtpClientHelper>();
        services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

        services.AddHttpContextAccessor();

        services.AddSession();
        services.AddLogging();

        services.AddCachingModule(configuration, appSettings);

        services.AddServicesModule(configuration);

        // ====================================
        // Contexts Cofig
        // ====================================

        // var connectionString = configuration.GetConnectionString("Sindvamb");
        // Replace with your server version and type.
        // Use 'MariaDbServerVersion' for MariaDB.
        // Alternatively, use 'ServerVersion.AutoDetect(connectionString)'.
        // For common usages, see pull request #1233.
        // var serverVersion = new MySqlServerVersion(new Version(8, 0, 32));

        // Replace 'YourDbContext' with the name of your own DbContext derived class.



        // ====================================
        // Events Config
        // ====================================

        services.AddScoped<IDomainNotification, DomainNotification>();
        services.AddScoped(typeof(IGenericNotificationHandler<>), typeof(GenericNotificationHandler<>));

        // Mediator
        services.AddScoped<IMediatorHandler, MediatorHandler>();

        // Notifications
        //services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
        services.AddScoped<INotificationHandler<DomainNotification>, GenericNotificationHandler<DomainNotification>>();

        // Events
        // services.AddScoped<INotificationHandler<Event>, EventHandler>();

        // Event Sourcing
        services.AddSingleton<IEventStoreService, EventStoreService>();
        services.AddSingleton<IEventSourcingRepository, EventSourcingRepository>();

        // ====================================
        // Modules
        // ====================================


        // ====================================
        // Services
        // ====================================



        if (assemblie.IsNotNull())
        {
            DependencyInjectionHelper.AddByConvention(services, assemblie);
        }

        return services;
    }
}
