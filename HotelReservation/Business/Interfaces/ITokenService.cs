

namespace Business.Interfaces
{
    public interface ITokenService
    {
         string BuildToken(string key, string email,string roleName,int id);
    }
}