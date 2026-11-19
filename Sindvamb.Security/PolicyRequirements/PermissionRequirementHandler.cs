#region

using Microsoft.AspNetCore.Authorization;
using Sindvamb.Security.Configurations;

#endregion

namespace Sindvamb.Security.PolicyRequirements;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        if (context.User == null || !context.User.Identity.IsAuthenticated)
        {
            context.Fail();
            return;
        }

        switch (requirement.Permission)
        {
            case AuthPolicy.MASTER:
                await PermissionMaster(context, requirement);
                break;
            case AuthPolicy.ADMIN:
                await PermissionAdmin(context, requirement);
                break;
            case AuthPolicy.SUPPORT:
                await PermissionSopport(context, requirement);
                break;
            case AuthPolicy.CUSTOMER:
                await PermissionEcommerce(context, requirement);
                break;
            default:
                await PermissionApplication(context, requirement);
                break;
        }

        context.Succeed(requirement);
    }

    private async Task PermissionApplication(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        context.Succeed(requirement);
    }

    private async Task PermissionEcommerce(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        context.Succeed(requirement);
    }

    private async Task PermissionSupplier(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        context.Succeed(requirement);
    }

    private async Task PermissionParthner(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        context.Succeed(requirement);
    }

    private async Task PermissionSopport(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        context.Succeed(requirement);
    }

    private async Task PermissionSeller(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        context.Succeed(requirement);
    }

    private async Task PermissionManager(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        context.Succeed(requirement);
    }

    private async Task PermissionMaster(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        context.Succeed(requirement);
    }

    private async Task PermissionAdmin(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        context.Succeed(requirement);
    }


    private async Task PermissionUser(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        //await AppServices.ExecuteAsync(GetUsers);
    }
}
