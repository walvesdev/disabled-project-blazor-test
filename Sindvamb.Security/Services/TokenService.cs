using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Sindvamb.Domain.Entities;
using Sindvamb.Utils.Services.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Sindvamb.Security.Services
{
    public class TokenService : ITransientDependency
    {
        public IConfiguration Config { get; }
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            Config = config;
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Name)
            };

            var key = Encoding.UTF8.GetBytes(Config["TokenKey"]);

            var securityKeySecret = new SymmetricSecurityKey(key);

            var signingCredentials = new SigningCredentials(securityKeySecret, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = signingCredentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
    }
}