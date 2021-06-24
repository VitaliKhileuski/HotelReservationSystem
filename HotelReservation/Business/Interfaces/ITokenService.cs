namespace Business.Interfaces
{
    public interface ITokenService
    {
         string BuildToken(string email,string roleName,string firstname,int id);
         string GenerateRefreshToken();
    }
}