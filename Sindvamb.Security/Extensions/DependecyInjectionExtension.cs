#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sindvamb.Security.PolicyRequirements;

#endregion

namespace Sindvamb.Security.Extensions;

public static class DependecyInjectionExtension
{
    public static IServiceCollection AddSecurityModule(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementHandler>();
        services.AddScoped<IAuthorizationHandler, RolesAuthorizationHandler>();
        // services.AddScoped<IAuthorizationHandler, PoliciesAuthorizationHandler>();  


        return services;
    }
}
