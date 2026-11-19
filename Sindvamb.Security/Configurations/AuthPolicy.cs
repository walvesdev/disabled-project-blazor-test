namespace Sindvamb.Security.Configurations;

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



