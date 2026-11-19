using System.Diagnostics;
using System.Threading;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;

namespace Sindvamb.Utils
{
    public static class AppServices
    {
        public static int APP_MASTER_ID = 2;

        private static IServiceProvider _services;

        public static IConfiguration Configuration { get; set; }

        public static IServiceCollection ServiceCollection { get; set; }
        
        public static string EnvironmentName => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;

        #region WebRootPath
        public static string WebRootPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");

        public static string WebRootTempPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "temp");
        public static string WebRootImgPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "img");

        public static void CreateWebRootTempPath()
        {
            if (!Directory.Exists(WebRootTempPath))
            {
                Directory.CreateDirectory(WebRootTempPath);
            }
        }
        #endregion

        public static IServiceProvider? ServiceProvider
        {
            get
            {
                if (_services == null) BuildServiceProvider();

                return _services;
            }
        }

        public static T GetService<T>(params object[] parameters) where T : class, new() => Activator.CreateInstance(typeof(T), parameters) as T;

        public static T GetService<T>() where T : class => ServiceProvider.GetRequiredService<T>();

        private static void BuildServiceProvider() => _services = ServiceCollection.BuildServiceProvider();

        public static HttpContext GetHttpContext()
        {
            return ServiceProvider!.GetRequiredService<IHttpContextAccessor>().HttpContext!;
        }

        public static List<string> GetRoles() => GetClaims<List<string>>("role");

        public static string? GetUserInfo() => string.Empty;

        public static async Task ExecuteAsync(Delegate @delegate, CancellationToken cancellationToken = default)
        {
            await ServiceProvider.ExecuteAsync(@delegate, cancellationToken);
        }

        public static void Execute(Delegate @delegate)
        {
            ServiceProvider.Execute(@delegate);
        }

        // public static bool IsDevelopment() => EnvironmentName.Equals("Development");
        public static bool IsDevelopment()
        {
            
            #if DEBUG
             return true;
            #endif
            
            return false;
        }
        
        public static string SiteUrl()
        {
            
#if DEBUG
            return "https://localhost:7096";
#endif
            
            return "https://www.sindvamb.com.br";
        }

        public static bool IsStaging() => EnvironmentName.Equals("Staging");

        public static bool IsProduction() => EnvironmentName.Equals("Production");

        public static bool IsAdmin() => GetRoles()?.Contains("ADMIN") ?? false;

        public static int GetTenantId() => int.Parse(GetClaims<List<string>>("Tenant").FirstOrDefault());

        public static T GetUserId<T>() => GetClaim<T>("Sub");

        public static T GetClaims<T>(string role = "role")
        {
            var ctx = GetHttpContext();

            var user = ctx.User;

            if (user == null) return default;

            var claims = (from claim in user.Claims
                where claim.Type.Contains(role) && !string.IsNullOrWhiteSpace(claim.Value)
                select claim.Value).ToList();
            var result = (T)Convert.ChangeType(claims, typeof(T));
            return result;
        }

        public static T GetClaim<T>(string role)
        {
            var ctx = GetHttpContext();

            if (ctx == null) return default;

            var user = ctx.User;

            if (user == null) return default;

            var claim = user.Claims.FirstOrDefault(p => p.Type.Contains(role)).Value;

            if (!string.IsNullOrWhiteSpace(claim)) return (T)Convert.ChangeType(claim, typeof(T));

            return default;
        }

        public static string GetRootDirectory()
        {
            var rootDirectory = AppContext.BaseDirectory;
            if (rootDirectory.Contains("bin")) rootDirectory = rootDirectory.Substring(0, rootDirectory.IndexOf("bin"));

            return rootDirectory;
        }

        public static string TakesExecutionTime(Func<dynamic> func)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            func();

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            var ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            var elapsedTime = string.Format("Elapsed: {0:00}:{1:00}:{2:00}.{3:00} (HH:MM:SS:MS)",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            return elapsedTime;
        }

        public static string TakesExecutionTime<T>(Func<T, dynamic> func, T param)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            func(param);

            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            var ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            var elapsedTime = string.Format("Elapsed: {0:00}:{1:00}:{2:00}.{3:00} (HH:MM:SS:MS)",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);

            return elapsedTime;
        }

        public static string GetSection<T>(string sectionKey)
        {
            return Configuration.GetSection(sectionKey).Value;
        }

        public static AppSettingsSection GetAppSettingsSection()
        {
            return Configuration.GetSection("AppSettings").Get<AppSettingsSection>();
        }

        public static void Log(LogData logData)
        {
            var logger = GetService<ILogger>();

            logger.LogError(logData.Serialize());
        }

        public static string GetSessionId()
        {
            var httpContextAccessor = GetService<IHttpContextAccessor>();
            var seesion = httpContextAccessor.HttpContext.Session;
            return seesion.Id;
        }

        public static string? GetIpAddress()
        {
            var ctx = GetHttpContext();
            var request = ctx?.Request;
            var ip = request?.GetIpAddress();
            return ip;
        }

        public static string GetUserMatricula()
        {
            return GetClaim<string>("Matricula");
        }
        
        public static string GetUserEmail()
        {
            return GetClaim<string>("Email");
        }

    }
}
