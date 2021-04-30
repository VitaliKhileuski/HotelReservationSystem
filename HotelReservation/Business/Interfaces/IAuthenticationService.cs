using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> Login(LoginUserRequestModel user);
        Task<string> Registration(RegisterUserRequestModel user);
    }
}
