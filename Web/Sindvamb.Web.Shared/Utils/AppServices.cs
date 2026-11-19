using Sindvamb.Web.Shared.Utils.CustomExceptions;

using System.Diagnostics;
using System.Net.Sockets;
using System.Security.Claims;


namespace Sindvamb.Web.Shared.Utils;

public static class AppServices
{
    public static int APP_MASTER_ID = 2;

    private static IServiceProvider? _services;

    public static Guid CredenciamentoCustomerId { get; set; }
    public static Guid CredenciamentoUserId { get; set; }

    public static IConfiguration Configuration { get; set; }

    public static bool ShowSteps { get; set; } = true;

    public static JwtAccessToken JwtAccessToken { get; set; } = new();

    public static Guid UserId => GetClaim<Guid>("Sub");

    public static IServiceCollection ServiceCollection { get; set; }

    public static ClaimsPrincipal User { get; set; }

    public static NavigationManager NavigationManager { get; set; }

    public static string? EnvironmentName { get; set; } = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    public static bool IsDevelopment => EnvironmentName == "Development";
    public static bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    #region WebRootPath
    public static string WebRootPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");

    public static string WebRootTempPath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "temp");

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

    public static KeyValuePair<string, string> ExceptionToThrow { get; set; } = new();

    public static T GetService<T>(params object[] parameters) where T : class, new() =>
        Activator.CreateInstance(typeof(T), parameters) as T;

    public static T GetService<T>() where T : class
    {
        var service = ServiceProvider.GetRequiredService<T>();
        return service;
    }

    private static void BuildServiceProvider() => _services = ServiceCollection.BuildServiceProvider();

    public static string GetLocalIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new CustomException("No network adapters with an IPv4 address in the system!");
    }

    public static void SetLogErrors<T>(Exception? exception = null,
        HttpStatusCode statusCode = HttpStatusCode.BadRequest, string? message = null)
        where T : class, new()
    {
        var logger = GetService<ILogger<T>>();

        string? GetEntityName()
        {
            var generic = typeof(T);
            var instance = Activator.CreateInstance(generic);
            var typeName = instance?.GetType().Name;
            return typeName;
        }

        if (exception != null)
            logger.LogError(new
            {
                userMessage = !string.IsNullOrWhiteSpace(message) ? message : null,
                statusCode,
                //userInfo = GetUserInfo(),
                responseModel = GetEntityName(),
                exceptionmessage = exception.Message,
                innerMessage = exception.InnerException?.Message,
                source = exception.Source,
                stackTrace = exception.StackTrace
            }.ToString());
        else
            logger.LogError(new
            {
                userMessage = !string.IsNullOrWhiteSpace(message) ? message : null,
                statusCode,
                //userInfo = GetUserInfo(),
                responseModel = GetEntityName()
            }.ToString());
    }

    public static KeyValuePair<string, string> GetErrorMessage(Guid errorId)
    {
        return ExceptionToThrow;
    }

    public static dynamic? GetClaim<T>(string role)
    {
        if (User.IsNull())
        {
            return default(T);
        }

        var claim = User?.Claims?.FirstOrDefault(p => p.Type.Contains(role))?.Value;

        if (!claim.IsNullOrWhiteSpace() && Guid.TryParse(claim, out var claaimValue))
        {
            return claaimValue;
        }

        if (!claim.IsNullOrWhiteSpace())
        {
            return (T)Convert.ChangeType(claim, typeof(T));
        }

        return default(T);
    }

    public static T GetClaims<T>(string role = "role")
    {
        if (User == null) return default;

        var claims = (from claim in User.Claims
                      where claim.Type.Contains(role) && !string.IsNullOrWhiteSpace(claim.Value)
                      select claim.Value).ToList();
        var result = (T)Convert.ChangeType(claims, typeof(T));
        return result;
    }

    public static List<string> GetRoles() => GetClaims<List<string>>("role");

    public static bool IsInRole(string roleToSearch)
    {
        var roles = GetRoles();

        if (roles.IsNullOrEmpty())
        {
            return false;
        }

        foreach (var r in roles)
        {
            if (roleToSearch.Contains(r))
            {
                return true;
            }
        }

        return false;
    }

    public static T? GetRole<T>(string roleToSearch)
    {
        var roles = GetRoles();

        if (roles.IsNullOrEmpty())
        {
            return default;
        }

        var role = roles.FirstOrDefault(role => role.Contains(roleToSearch));

        if (role is not null)
        {
            return (T)Convert.ChangeType(role, typeof(T))!;
        }

        return default;
    }

    public static bool IsAdmin() => IsInRole(AuthRole.ADMIN);

    public static bool IsMaster() => IsInRole(AuthRole.MASTER);

    public static bool IsSupport() => IsInRole(AuthRole.SUPPORT);

    public static bool IsCustomer() => IsInRole(AuthRole.CUSTOMER);

    public static async Task ExecuteAsync(Delegate @delegate, CancellationToken cancellationToken = default)
    {
        await ServiceProvider.ExecuteAsync(@delegate, cancellationToken);
    }

    public static void Execute(Delegate @delegate)
    {
        ServiceProvider.Execute(@delegate);
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
        return Configuration!.GetSection(sectionKey)!.Value!;
    }

}
