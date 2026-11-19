using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;
using AutoMapper;
using AutoMapper.Data;
using DinkToPdf.Contracts;
using DinkToPdf;
using EventStore.Core.Util;
using Hangfire;
using Hangfire.Redis.StackExchange;
using Hangfire.SqlServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Sindvamb.Application.Core.Auth;
using Sindvamb.Application.Core.Conventions;
using Sindvamb.Application.Core.Extensions;
using Sindvamb.Application.Core.FIlters;
using Sindvamb.Application.Host.SeedData;
using Sindvamb.Repository.Extensions;
using Sindvamb.Security.Extensions;
using Sindvamb.Utils;
using StackExchange.Redis;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Hangfire.PostgreSql;
using System.Globalization;

namespace Sindvamb.Application.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.WebHost.UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging(logging =>
                {
                    logging.AddLog4Net(new Log4NetProviderOptions("log4net.config"));
                })
                .ConfigureKestrel(options =>
                {
                    options.Limits.MaxRequestBodySize = long.MaxValue;
                    options.Limits.MaxRequestBufferSize = long.MaxValue;
                })
                .ConfigureAppConfiguration((webHostBuilderContext, configBuilder) =>
                {

                    var env = webHostBuilderContext.HostingEnvironment;
                    var root = Directory.GetCurrentDirectory();
                    var tmp = Path.Combine(root, "tmp");

                    //configBuilder.AddCertificates(root);

                    if (Directory.Exists(tmp))
                    {
                        Directory.Delete(tmp, true);
                    }

                    Directory.CreateDirectory(tmp);

#if STAGING
                            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Staging");
                            env.EnvironmentName = "Staging";
#endif
#if HML
                            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "HML");
                            env.EnvironmentName = "HML";
#endif
#if DEBUG
                    Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
                    env.EnvironmentName = "Development";
#endif
#if DOCKER
                            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Docker");
                            env.EnvironmentName = "Docker";
#endif
#if PRODUCTION
                            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
                            env.EnvironmentName = "Production";
#endif
#if RELEASE
                            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
                            env.EnvironmentName = "Production";
#endif

                    configBuilder
                        .AddJsonFile(Path.Combine(root, "appsettings.json"), optional: false, reloadOnChange: true)
                        .AddJsonFile(Path.Combine(root, $"appsettings.{env.EnvironmentName}.json"), optional: false,
                            reloadOnChange: true);

                    if (env.EnvironmentName == "Docker")
                    {
                        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
                        env.EnvironmentName = "Development";
                    }

                    if (env.EnvironmentName == "Docker")
                    {
                        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
                        env.EnvironmentName = "Production";
                    }

                    configBuilder.AddEnvironmentVariables();
                });


            AppServices.ServiceCollection = builder.Services;
            AppServices.Configuration = builder.Configuration;

            var appSettings = builder.Configuration.GetSection("AppSettings").Get<AppSettingsSection>();

            var assembly = typeof(Program).GetTypeInfo().Assembly;

            builder.Services.AddTransient<Seed>();

            builder.Services.AddSession();

            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly);
                cfg.Lifetime = ServiceLifetime.Scoped;
            });

            builder.Services.AddDependencyInjection(builder.Configuration, assembly, appSettings);

            builder.Services.AddSecurityModule(builder.Configuration);

            builder.Services.AddJwtConfigWebApi(builder.Configuration);

            var mapprConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(new string[]
                {
                    "Sindvamb.Domain"
                });

                cfg.AddDataReaderMapping();
            });

            var mapper = mapprConfiguration.CreateMapper();
            builder.Services.AddSingleton(mapper);

            builder.Services.AddHttpContextAccessor();

            JsonConvert.DefaultSettings = () =>
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver(),
                    Formatting = Formatting.None,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore
                };
                settings.Converters.Add(new StringEnumConverter());
                return settings;
            };

            builder.Services.AddControllers(opt =>
            {
                opt.AllowEmptyInputInBodyModelBinding = true;
                opt.Filters.Add(typeof(CustomExceptionFilter));
                opt.Filters.Add(typeof(ModelStateValidationFilter));
                opt.RespectBrowserAcceptHeader = true;
                opt.Conventions.Add(new ControllerDocumentationConvention());
                opt.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
                opt.AllowEmptyInputInBodyModelBinding = true;
            })
                .ConfigureApiBehaviorOptions(o => { o.SuppressModelStateInvalidFilter = true; })
                .AddJsonOptions(opts =>
                {
                    opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    opts.JsonSerializerOptions.PropertyNamingPolicy = null;
                })
                .AddNewtonsoftJson(opts =>
                {
                    var settings = JsonConvert.DefaultSettings();

                    opts.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    opts.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    opts.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    opts.SerializerSettings.DefaultValueHandling = settings.DefaultValueHandling;
                    opts.SerializerSettings.TraceWriter = new TraceWriter();


                    foreach (var converter in settings.Converters)
                    {
                        opts.SerializerSettings.Converters.Add(converter);
                    }

                    opts.SerializerSettings.Formatting = Formatting.Indented;
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddRepositories(builder.Configuration);

            var cors = appSettings.Security.CorsOrigins.ToArray();

            builder.Services.AddCors();

            if (appSettings.UseCache)
            {
                if (appSettings.Redis.UseRedis)
                {
                    var redisConnectionString =
                        appSettings.Redis.GetConnectionString(AppSettingsSection.RedisConnectionSection.ConnectionType
                            .Default);
                    var redis = ConnectionMultiplexer.Connect(redisConnectionString);

                    GlobalConfiguration.Configuration
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseIgnoredAssemblyVersionTypeResolver()
                        .UseSerializerSettings(new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Objects,
                        })
                        .UseStorage(new RedisStorage(redis, new RedisStorageOptions
                        {
                            Db = appSettings.Hangfire.Redis.Db,
                            Prefix = appSettings.Hangfire.Redis.Prefix
                        }));
                }
                else
                {
                    GlobalConfiguration.Configuration
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseIgnoredAssemblyVersionTypeResolver()
                        .UseSerializerSettings(new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All,
                        })
                        // .Uses(c => c.UseNpgsqlConnection((builder.Configuration.GetConnectionString("Hangfire"))));
                        .UseSqlServerStorage(builder.Configuration.GetConnectionString("Hangfire"),
                            new SqlServerStorageOptions
                            {
                                DisableGlobalLocks = true, // Defaults to `true`
                            });
                }
            }

            var cultureName = "pt-BR";
            var culture = new CultureInfo(cultureName);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            var app = builder.Build();

            app.UseSession();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Sindvamb API");
                c.RoutePrefix = string.Empty; // Set Swagger UI at apps root
            });


            // global cors policy
            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .SetIsOriginAllowed(origin => true) // allow any origin
                .WithOrigins(cors) // Allow only this origin can also have multiple origins separated with comma
                .AllowCredentials()); // allow credentials

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            var seed = AppServices.GetService<Seed>();

            Task.Run(async () => await seed.Start());

            app.Run();
        }
    }

    class TraceWriter : Newtonsoft.Json.Serialization.ITraceWriter
    {
        public TraceLevel LevelFilter
        {
            get
            {
                return TraceLevel.Error;
            }
        }

        public void Trace(TraceLevel level, string message, Exception ex)
        {
            Console.WriteLine("JSON {0} {1}: {2}", level, message, ex);
        }
    }
}