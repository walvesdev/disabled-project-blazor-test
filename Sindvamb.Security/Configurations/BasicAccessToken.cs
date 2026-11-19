#region

using Microsoft.AspNetCore.Authentication;

#endregion

namespace Sindvamb.Security.Configurations;


public class BasicAccessToken
{
    public bool Authenticated { get; set; }
    public string Created { get; set; }
    public string Expiration { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
    public Guid? Code { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool Success { get; set; }
    public AuthenticationTicket? AuthenticationTicket { get; set; }
}

public class JwtAccessToken
{
    public bool Authenticated { get; set; }
    public string Created { get; set; }
    public string Expiration { get; set; }
    public string Message { get; set; }
    public string Token { get; set; }
    public Guid? Code { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public bool Success { get; set; }
}