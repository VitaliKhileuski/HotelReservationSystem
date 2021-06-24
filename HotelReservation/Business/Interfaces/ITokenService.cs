using System;

namespace Business.Interfaces
{
    public interface ITokenService
    {
         string BuildToken(string email,string roleName,string firstname,string id);
         string GenerateRefreshToken();
    }
}