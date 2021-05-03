using System.Threading.Tasks;
using Business.Models.RequestModels;

namespace Business.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> Login(LoginUserRequestModel user);
        Task<string> Registration(RegisterUserRequestModel user);
    }
}