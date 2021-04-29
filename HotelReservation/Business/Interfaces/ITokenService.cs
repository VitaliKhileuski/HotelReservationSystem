using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Interfaces
{
    public interface ITokenService
    {
         string BuildToken(string key, string email);
    }
}
