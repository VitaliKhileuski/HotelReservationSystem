using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Repositories
{
    public class RoomRepository : BaseRepository<RoomEntity>
    {
        private readonly Context _db;

        public RoomRepository(Context context) : base(context)
        {
            _db = context;
        }
    }
}
