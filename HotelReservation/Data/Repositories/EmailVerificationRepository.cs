using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class EmailVerificationRepository : BaseRepository<EmailVerificationEntity>, IEmailVerificationRepository
    {
        private readonly Context _db;

        public EmailVerificationRepository(Context context) : base(context)
        {
            _db = context;
        }
    }
}
