namespace Sindvamb.Web.Shared.Utils;

public class BasicAccessToken
{
    public bool Authenticated { get; set; }
    public string Created { get; set; }
    public string Expiration { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
    public Guid? Code { get; set; }
    public Guid? Tenant { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool Success { get; set; }
}

public class JwtAccessToken
{
    public bool Authenticated { get; set; }
    public string Created { get; set; }
    public string Expiration { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
    public Guid? Code { get; set; }
    public Guid? Tenant { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool Success { get; set; }
}
