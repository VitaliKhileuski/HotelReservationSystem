using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class EmailVerificationRepository : BaseRepository<EmailVerificationEntity>, IEmailVerificationRepository
    {
        private readonly Context _db;

        public EmailVerificationRepository(Context context) : base(context)
        {
            _db = context;
        }

        public IQueryable<EmailVerificationEntity> FindByUserId(Guid userId)
        {
            var emailVerificationEntities = _db.EmailVerificationEntities.Where(x => x.UserId == userId);
            return emailVerificationEntities;
        }
    }
}
