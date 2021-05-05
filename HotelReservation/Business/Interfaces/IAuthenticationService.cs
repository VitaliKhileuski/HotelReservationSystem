using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> Login(LoginUserModel user);
        Task<string> Registration(RegisterUserModel user);
    }
}