#region

using Microsoft.AspNetCore.Authorization;

#endregion

namespace Sindvamb.Utils.Attributes;

public class PermissionAttribute : AuthorizeAttribute
{
    const string POLICY_PREFIX = ""; //""Permission";

    public PermissionAttribute(string requiredPermission) =>
        RequiredPermission = requiredPermission;

    public string? RequiredPermission
    {
        get { return Policy?.Substring(POLICY_PREFIX.Length); }
        set { Policy = $"{POLICY_PREFIX}{value}"; }
    }
}
