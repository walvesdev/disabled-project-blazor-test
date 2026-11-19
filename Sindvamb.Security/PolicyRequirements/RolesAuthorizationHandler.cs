#region

using Sindvamb.Security.Configurations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Sindvamb.Utils;
using Sindvamb.Utils.Extension;

#endregion

namespace Sindvamb.Security.PolicyRequirements;

public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        RolesAuthorizationRequirement requirement)
    {
        if (context.User == null || !context.User.Identity.IsAuthenticated)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        var validRole = false;
        if (requirement.AllowedRoles == null ||
            requirement.AllowedRoles.Any() == false)
        {
            validRole = true;
        }
        else
        {
            var roles = AppServices.GetRoles();

            var role = AuthRole.Roles.FirstOrDefault(t => t.Key == roles.FirstOrDefault()).Value;

            if (!role.IsNullOrWhiteSpace())
            {
                foreach (var allowedRoles in requirement.AllowedRoles)
                {
                    if (role.Contains(','))
                    {
                        role.Split(",").ForEach(r =>
                        {
                            if (r.Contains(allowedRoles))
                            {
                                validRole = true;
                            }
                        });
                    }
                    else
                    {
                        if (allowedRoles.Contains(role))
                        {
                            validRole = true;
                        }
                    }
                }
            }
        }

        if (validRole)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}