#region

using System.ComponentModel;
using System.Reflection;
using System.Text.Json.Serialization;
using AutoMapper;
using AutoMapper.Data;
using DinkToPdf;
using DinkToPdf.Contracts;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Redis.StackExchange;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Sindvamb.Application.Core.Attributes;
using Sindvamb.Application.Core.Conventions;
using Sindvamb.Application.Core.Extensions;
using Sindvamb.Application.Core.FIlters;
using Sindvamb.Caching.Extensions;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using Sindvamb.HangFire.Jobs.Extensions;
using Sindvamb.Repository.Extensions;
using Hangfire.PostgreSql;
using Sindvamb.HangFire.Core.Filters;
using Sindvamb.Utils.Resolvers;

#endregion

namespace Sindvamb.HangFire.Host
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public int ApiVersion { get; set; } = 1;

        public void ConfigureServices(IServiceCollection services)
        {
            AppServices.ServiceCollection = services;
            AppServices.Configuration = Configuration;

            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettingsSection>();

            var MapprConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(new string[]
                {
                    "Sindvamb.Domain"
                });

                cfg.AddDataReaderMapping();
            });

            var mapper = MapprConfiguration.CreateMapper();

            services.AddSingleton(mapper);


            var hangfireAssembly = typeof(Startup).GetTypeInfo().Assembly;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(Startup).Assembly);
                cfg.Lifetime = ServiceLifetime.Scoped;
            });

            services.AddSignalR();
            services.AddDependencyInjection(Configuration, hangfireAssembly, appSettings);
            services.AddRepositories(Configuration);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            var connectionString = Configuration.GetConnectionString("Hangfire");


            if (appSettings.Redis.UseRedis)
            {
                var redisConnectionString = appSettings.Redis.GetConnectionString(AppSettingsSection.RedisConnectionSection.ConnectionType.Default);
                var redis = ConnectionMultiplexer.Connect(redisConnectionString);

                services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseMediatR());

                GlobalConfiguration.Configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseIgnoredAssemblyVersionTypeResolver()
                    .UseSerializerSettings(new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    })
                    .UseStorage(new RedisStorage(redis, new RedisStorageOptions
                    {
                        Db = appSettings.Hangfire.Redis.Db,
                        Prefix = appSettings.Hangfire.Redis.Prefix
                    }));

            }
            else
            {
                services.AddHangfire(configuration => configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(connectionString)
                    .UseMediatR());

                GlobalConfiguration.Configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseIgnoredAssemblyVersionTypeResolver()
                    .UseSerializerSettings(new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    })
                    .UsePostgreSqlStorage(c => c.UseNpgsqlConnection((Configuration.GetConnectionString("Hangfire"))));

                //GlobalConfiguration.Configuration
                //    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                //    .UseSimpleAssemblyNameTypeSerializer()
                //    .UseRecommendedSerializerSettings()
                //    .UseIgnoredAssemblyVersionTypeResolver()
                //    .UseSerializerSettings(new JsonSerializerSettings
                //    {
                //        TypeNameHandling = TypeNameHandling.All,
                //    })
                //    .UseSqlServerStorage(Configuration.GetConnectionString("Hangfire"), new SqlServerStorageOptions
                //    {
                //        DisableGlobalLocks = true, // Defaults to `true`
                //    });


            }

            //services.AddApiVersioning(o =>
            //{
            //    o.ReportApiVersions = true;
            //    o.AssumeDefaultVersionWhenUnspecified = true;
            //    o.DefaultApiVersion = new ApiVersion(ApiVersion, 0);
            //});

            //services.AddVersionedApiExplorer(o =>
            //{
            //    o.GroupNameFormat = "'v'VVV";
            //    o.SubstituteApiVersionInUrl = true;
            //});

            services.AddSwaggerGen(swagger =>
            {
                swagger.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);

                swagger.SwaggerDoc($"v{ApiVersion}", new OpenApiInfo
                {
                    Title = "Sindvamb API",
                    Version = $"v{ApiVersion}"
                });

                // swagger.OperationFilter<RemoveVersionFromParameter>();

                swagger.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo))
                    {
                        return false;
                    }

                    IEnumerable<ApiSettingsAttribute> versions = methodInfo.DeclaringType
                        .GetCustomAttributes(true)
                        .OfType<ApiSettingsAttribute>()
                        .Select(a => a);

                    if (AppServices.IsDevelopment())
                    {
                        return versions.Any(v => $"v{v.Version.ToString()}" == docName && !v.Hidden);
                    }

                    return versions.Any(v => $"v{v.Version.ToString()}" == docName && !v.Hidden && !v.HiddenProduction && !v.HiddenStaging && !v.HiddenHML);
                });

                swagger.CustomSchemaIds(x => x.GetCustomAttributes<DisplayNameAttribute>()?.SingleOrDefault()?.DisplayName ?? x.FullName);

                swagger.DescribeAllParametersInCamelCase();

                var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory).Where(name => name.EndsWith(".xml"));

                if (!files.IsNullOrEmpty())
                {
                    foreach (var file in files)
                    {
                        if (File.Exists(file))
                        {
                            swagger.IncludeXmlComments(file, includeControllerXmlComments: true);
                        }
                    }
                }

                swagger.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "basic"
                            }
                        },
                        new string[] { }
                    }
                });
            });

            // services.AddEndpointsApiExplorer();
            services.AddHttpClient();
            services.AddLogging();


            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            if (AppServices.IsDevelopment())
            {
                services.AddHangfireServer(options =>
                {
                    options.ServerName = Environment.MachineName;
                    options.Queues = new[] { MachineQueueAttribute.MachineQueueName, "default" };
                    options.TimeZoneResolver = new TimeZoneConverterResolver();
                });

                GlobalJobFilters.Filters.Add(new MachineQueueAttribute());
                GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });

                GlobalConfiguration.Configuration.UseFilter(new MachineQueueAttribute());
                GlobalConfiguration.Configuration.UseFilter(new AutomaticRetryAttribute { Attempts = 0 });
            }
            else
            {
                services.AddHangfireServer(options =>
                {
                    options.ServerName = Environment.MachineName;
                    options.TimeZoneResolver = new TimeZoneConverterResolver();
                });
            }

            //services.AddDataProtectionAndRedis(appSettings);
            services.AddRazorPages();
            services.AddControllersWithViews(opt =>
                {
                    opt.AllowEmptyInputInBodyModelBinding = true;
                    opt.Filters.Add(typeof(CustomExceptionFilter));
                    opt.Filters.Add(typeof(ModelStateValidationFilter));
                    opt.RespectBrowserAcceptHeader = true;
                    opt.Conventions.Add(new ControllerDocumentationConvention());
                })
                .ConfigureApiBehaviorOptions(o => { o.SuppressModelStateInvalidFilter = true; })
                .AddJsonOptions(opts => { opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); })
                .AddNewtonsoftJson(opts =>
                {
                    var settings = new JsonSerializerSettings();

                    opts.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    opts.SerializerSettings.DefaultValueHandling = settings.DefaultValueHandling;


                    foreach (var converter in settings.Converters)
                    {
                        opts.SerializerSettings.Converters.Add(converter);
                    }

                    opts.SerializerSettings.Formatting = Formatting.Indented;
                });

            services.Configure<AppSettingsSection>(Configuration.GetSection("AppSettings"));

            services.AddDataProtectionAndRedis(appSettings);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IRecurringJobManager recurringJob, IBackgroundJobClient jobClient)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettingsSection>();

            app.UseRouting();
            app.UseAuthentication();

            app.UseHangfireDashboard(pathMatch: "/dashboard", options: new DashboardOptions
            {
                Authorization = new[] { new CustomAuthorizationFilter(appSettings.LoginUrl, appSettings.CookieName) },
                AppPath = appSettings.LoginUrl,
                DisplayStorageConnectionString = false,
                TimeZoneResolver = new TimeZoneConverterResolver()
            });

            var options = new DashboardOptions
            {
                Authorization = new[] { new MyAuthorizationFilter() }
            };

            //app.UseHangfireDashboard("/dashboard", options);


            //app.UseHangfireDashboard(pathMatch: "/dashboard");
            app.UseWelcomePage("/");



            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
            });

            if (env.IsDevelopment())
            {
                recurringJob.RemoveIfExists("Yesterday");
            }

            // Cron poder ser assim: "*/30 * * * *"
            // ou assim: Cron.Daily(18)

            // adiciona os jobs recorrentes
            recurringJob.AddRecurringJobs();
        }
    }

    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context) => true;
    }
}