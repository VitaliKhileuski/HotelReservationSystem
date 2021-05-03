

namespace Business.Interfaces
{
    public interface ITokenService
    {
         string BuildToken(string key, string email);
    }
}