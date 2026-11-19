#region

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Sindvamb.Utils.Extension;

#endregion

namespace Sindvamb.Security.PolicyRequirements;

public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return Task.FromResult(
            new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser().Build());
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return Task.FromResult<AuthorizationPolicy?>(null);
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.IsNullOrWhiteSpace())
        {
            return Task.FromResult<AuthorizationPolicy?>(null);
        }

        var policy = new AuthorizationPolicyBuilder();
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new PermissionRequirement(policyName));
        return Task.FromResult(policy.Build());
    }
}
