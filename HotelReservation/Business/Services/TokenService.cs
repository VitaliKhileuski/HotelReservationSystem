using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Business.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services
{
    public class TokenService : ITokenService
    {
        private readonly AuthOptions _options;

        public TokenService(IOptions<AuthOptions> options)
        {
            _options = options.Value;
        }

        public string BuildToken(string email, string roleName,string firstname, int id)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[]
            {
                new Claim("email", email),
                new Claim("role", roleName),
                new Claim("firstname",firstname),
                new Claim("id", id.ToString())

            };
            DateTime expires = DateTime.Now.AddMinutes(Convert.ToDouble(_options.Lifetime));
            var jwt = new JwtSecurityToken(claims: claims,
                signingCredentials: credentials,
                expires: expires,
                audience:_options.Audience,
                issuer: _options.Issuer);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public string GenerateRefreshToken()
        {
            using var rng = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
    }
}