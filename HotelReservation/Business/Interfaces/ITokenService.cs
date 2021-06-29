using System;

namespace Business.Interfaces
{
    public interface ITokenService
    {
         string BuildToken(string email,string roleName,string firstname,Guid id);
         string GenerateRefreshToken();
    }
}