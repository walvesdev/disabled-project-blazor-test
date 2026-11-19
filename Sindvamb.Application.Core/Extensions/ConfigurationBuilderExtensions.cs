#region

using System.Reflection;
using System.Security.Cryptography.X509Certificates;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Sindvamb.Utils;
using Sindvamb.Utils.Filters.CustomExceptions;

#endregion

namespace Sindvamb.Application.Core.Extensions;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddCertificates(this IConfigurationBuilder builder, string root)
    {
        var pathCertIconePfx = Path.Combine(root, "sindvamb.pfx");

        var certPfx = X509CertificateLoader.LoadPkcs12FromFile(pathCertIconePfx, "33841172",
            X509KeyStorageFlags.UserKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);

        //When LocalMachine is used, .Add() requires that you run the app as an administrator in order to work.
        var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        store.Open(OpenFlags.MaxAllowed);
        store.Add(certPfx);
        store.Close();

        return builder;
    }

    public static WebApplicationBuilder BuildSettings(this WebApplicationBuilder builder, bool addLogging = false)
    {
        if (addLogging)
        {
            builder.Logging.AddLog4Net(new Log4NetProviderOptions("log4net.config"));
        }

        builder.Host.ConfigureHostConfiguration(webBuilder =>
        {
            var location = Assembly.GetEntryAssembly().Location ?? string.Empty;
            var executableDirectory = Path.GetDirectoryName(location) ?? string.Empty;

            if (string.IsNullOrWhiteSpace(executableDirectory))
            {
                throw new CustomException(new ValidationExceptionResult
                {
                    Message = "Erro importar arquivos settigns!",
                    Data = new
                    {
                        Location = location,
                        ExecutableDirectory = executableDirectory
                    }
                });
            }

            var env = builder.Environment;
            var files = Directory.GetFiles(executableDirectory).Where(f => f.ToLower().Contains("settings"));

            foreach (var file in files)
            {
                if (file.Contains("appsettings.json"))
                {
                    webBuilder.AddJsonFile(file, false, true);
                }

                if (file.Contains($"appsettings.{env.EnvironmentName}.json"))
                {
                    webBuilder.AddJsonFile(file, false, true);
                }

                if (file.Contains("CommonSettings.json"))
                {
                    webBuilder.AddJsonFile(file, false, true);
                }

                if (file.Contains($"CommonSettings.{env.EnvironmentName}.json"))
                {
                    webBuilder.AddJsonFile(file, false, true);
                }
            }

            webBuilder.AddCertificates(executableDirectory);

            webBuilder.AddEnvironmentVariables();
        });

        return builder;
    }

    public static string GetRootDirectory()
    {
        var rootDirectory = AppContext.BaseDirectory;
        if (rootDirectory.Contains("bin"))
        {
            rootDirectory = rootDirectory[..rootDirectory.IndexOf("bin", StringComparison.Ordinal)];
        }

        return rootDirectory;
    }
}
