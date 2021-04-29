using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Business.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Business.Services
{
    public class TokenService : ITokenService
    {

        public string BuildToken(string key, string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[]
            {
                new Claim("email", email)
            };
            DateTime expires = DateTime.Now;
            var jwt = new JwtSecurityToken(claims: claims, signingCredentials: credentials, expires: expires);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

    }
}
