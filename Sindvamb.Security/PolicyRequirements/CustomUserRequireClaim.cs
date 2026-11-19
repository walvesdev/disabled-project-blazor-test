#region

using Microsoft.AspNetCore.Authorization;

#endregion

namespace Sindvamb.Security.PolicyRequirements;

public class CustomUserRequireClaim : IAuthorizationRequirement
{
    public string ClaimType { get; }

    public CustomUserRequireClaim(string claimType)
    {
        ClaimType = claimType;
    }
}
