using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Repositories
{
    public class ImageRepository : BaseRepository<ImageEntity>
    {
        private readonly Context _db;

        public ImageRepository(Context context) : base(context)
        {
            _db = context;
        }
    }
}
