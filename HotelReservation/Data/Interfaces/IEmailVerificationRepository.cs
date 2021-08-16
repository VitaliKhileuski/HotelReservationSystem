using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IEmailVerificationRepository : IBaseRepository<EmailVerificationEntity>
    {
        IQueryable<EmailVerificationEntity> FindByUserId(Guid userId);
    }
}
