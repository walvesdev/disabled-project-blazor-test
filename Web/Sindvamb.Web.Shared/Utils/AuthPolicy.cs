using Microsoft.AspNetCore.Authorization;

namespace Sindvamb.Web.Shared.Utils;

public static class AuthPolicy
{
    public const string CUSTOMER = "CUSTOMER";
    public const string ADMIN = "ADMIN";
    public const string MASTER = "MASTER";
    public const string SUPPORT = "SUPPORT";
}

public static class AuthRole
{
    public const string CUSTOMER = "CUSTOMER";
    public const string ADMIN = "ADMIN";
    public const string ADMIN_MASTER = "ADMIN,MASTER";
    public const string ADMIN_MASTER_SUPPORT = "ADMIN,MASTER,SUPPORT";
    public const string ADMIN_MASTER_SUPPORT_CUSTOMER = "ADMIN,MASTER,SUPPORT,CUSTOMER";
    public const string MASTER = "MASTER";
    public const string SUPPORT = "SUPPORT";

    public static List<KeyValuePair<string, string>> Roles = new()
    {
        new KeyValuePair<string, string>("SUPPORT", SUPPORT),
        new KeyValuePair<string, string>("CUSTOMER", CUSTOMER),
        new KeyValuePair<string, string>("ADMIN", ADMIN),
        new KeyValuePair<string, string>("MASTER", MASTER),
    };
    
}

public static class Policies
{
    public const string ADMIN = "ADMIN";
    public const string MASTER = "MASTER";

    public static AuthorizationPolicy IsAdminPolicy()
    {
        return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
            .RequireRole("ADMIN")
            .Build();
    }

    public static AuthorizationPolicy IsMasterPolicy()
    {
        return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
            .RequireRole("MASTER")
            .Build();
    }

    //services.AddAuthorization(config =>
    //{
    //    config.AddPolicy(Policies.IsAdmin, Policies.IsAdminPolicy());
    //    config.AddPolicy(Policies.IsUser, Policies.IsUserPolicy());
    //});

    //[Authorize(Policy = Policies.IsAdmin)]
    //[Route("api/[controller]")]
    //public class SampleDataController : Controller

    //<AuthorizeView Policy = "@Policies.IsUser" >
    //    < p > You can only see this if you satisfy the IsUser policy.</p>
    //</AuthorizeView>

    //@attribute[Authorize(Policy = Policies.IsAdmin)]
}



