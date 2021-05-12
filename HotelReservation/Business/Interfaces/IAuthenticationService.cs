using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IAuthenticationService
    {
        Task<List<string>> Login(LoginUserModel user);
        Task<List<string>> Registration(RegisterUserModel user);
    }
}