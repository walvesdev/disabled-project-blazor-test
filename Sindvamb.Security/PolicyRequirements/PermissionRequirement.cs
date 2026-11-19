#region

using Microsoft.AspNetCore.Authorization;

#endregion

namespace Sindvamb.Security.PolicyRequirements;

public class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permission) =>
        Permission = permission;

    public string Permission { get; }
}
