using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IEmailSenderService
    {
        Task CreateEmailVerificationCode(Guid userId);
    }
}
