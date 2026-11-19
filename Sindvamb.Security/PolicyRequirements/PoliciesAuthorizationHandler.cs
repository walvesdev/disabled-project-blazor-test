

using Microsoft.AspNetCore.Authorization;
#region


#endregion

namespace Sindvamb.Security.PolicyRequirements;

public class PoliciesAuthorizationHandler : AuthorizationHandler<CustomUserRequireClaim>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        CustomUserRequireClaim requirement)
    {
        if (context.User == null || !context.User.Identity.IsAuthenticated)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var hasClaim = context.User.Claims.Any(x => x.Type == requirement.ClaimType);

        if (hasClaim)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        context.Fail();
        return Task.CompletedTask;
    }
}
