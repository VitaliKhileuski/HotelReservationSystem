using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _cfg;

        public TokenService(IConfiguration configuration)
        {
            _cfg = configuration;
        }
        public string BuildToken(string key, string email,string roleName)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[]
            {
                new Claim("email", email),
                new Claim("role",roleName)
            };
            DateTime expires = DateTime.Now.AddMinutes(Convert.ToDouble(_cfg["AuthenticationOptions:lifetime"]))+ new TimeSpan(0, 0, 0, 30);
            var jwt = new JwtSecurityToken(claims: claims,
                signingCredentials: credentials,
                expires: expires,
                audience:_cfg["AuthenticationOptions:audience"],
                issuer:_cfg["AuthenticationOptions:issuer"]);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}