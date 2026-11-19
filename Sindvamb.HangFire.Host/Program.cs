#region

using Hangfire;

#endregion

namespace Sindvamb.HangFire.Host
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseColouredConsoleLogProvider()
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings();

            CreateWebHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging => { logging.AddLog4Net(new Log4NetProviderOptions("log4net.config")); })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .ConfigureAppConfiguration((webHostBuilderContext, configBuilder) =>
                        {
                            var env = webHostBuilderContext.HostingEnvironment;
                            var root = Directory.GetCurrentDirectory();

                          

                            //configBuilder.AddCertificates(root);

                            IList<IConfigurationSource> sources = new List<IConfigurationSource>();

                            foreach (dynamic source in configBuilder.Sources)
                            {
                                var s = (source as IConfigurationSource);

                                if (s.ContainProperty("Path"))
                                {
                                    if (source.Path.Contains("appsettings"))
                                    {
                                        sources.Add(source);
                                    }
                                }

                                if (s.ContainProperty("Prefix"))
                                {
                                    sources.Add(source);
                                }
                            }

                            if (sources.Count > 0)
                            {
                                configBuilder.Sources.RemoveAll(sources);
                            }


#if STAGING
                            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Staging");
                            env.EnvironmentName = "Staging";
#endif

#if SANDBOX
                            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Sandbox");
                            env.EnvironmentName = "Sandbox";
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
                            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
                            env.EnvironmentName = "Docker";
#endif

#if PRODUCTION
                            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
                            env.EnvironmentName = "Production";
#endif


                            configBuilder
                                .AddJsonFile(Path.Combine(root, "appsettings.json"), optional: false, reloadOnChange: true)
                                .AddJsonFile(Path.Combine(root, $"appsettings.{env.EnvironmentName}.json"), optional: false, reloadOnChange: true);

                            if (env.EnvironmentName == "Docker")
                            {
                                env.EnvironmentName = "Development";
                            }

                            configBuilder.AddEnvironmentVariables();
                        })
                        .UseSockets()
                        .UseStartup<Startup>();
                });
    }
}