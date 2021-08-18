using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class ReviewRepository : BaseRepository<ReviewEntity>, IReviewRepository
    {
        private readonly Context _db;

        public ReviewRepository(Context context) : base(context)
        {
            _db = context;
        }
    }
}
