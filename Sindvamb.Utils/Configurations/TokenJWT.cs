#region

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Sindvamb.Utils;
using Sindvamb.Utils.Services.DependencyInjection;

#endregion

namespace Sindvamb.Utils.Configurations;

public class TokenConfigurations : ISingletonDependency
{
    public string Audience { get; set; }

    public string Issuer { get; set; }

    public int Lifetime { get; set; }

    public string SecurityKeySecret { get; set; }
}

public class SigningConfigurations : ISingletonDependency
{
    public SecurityKey Key => GetSecurityKey();

    public SigningCredentials SigningCredentials { get; private set; }

    public string SecurityKeySecret { get; set; }

    public SigningConfigurations()
    {
        GetSecurityKey();
    }


    public SymmetricSecurityKey GetSecurityKey()
    {
      
        var appSettings = AppServices.GetAppSettingsSection();

        SecurityKeySecret = appSettings.TokenConfigurations.SecurityKeySecret;

        var key = Encoding.ASCII.GetBytes(SecurityKeySecret);

        var securityKeySecret = new SymmetricSecurityKey(key);

        SigningCredentials = new SigningCredentials(securityKeySecret, SecurityAlgorithms.HmacSha256Signature);

        return securityKeySecret;
    }
}
