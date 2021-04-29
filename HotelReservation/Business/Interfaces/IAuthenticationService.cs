using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Business.DTO;

namespace Business.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> Login(LoginUserDTO user);
        Task<string> Registration(LoginUserDTO user);
    }
}
