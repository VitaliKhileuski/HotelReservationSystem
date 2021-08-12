using System;
using System.Collections.Generic;
using System.Text;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class ServiceQuantityRepository : BaseRepository<ServiceQuantityEntity>, IServiceQuantityRepository
    {
        private readonly Context _db;

        public ServiceQuantityRepository(Context context) : base(context)
        {
            _db = context;
        }
    }
}
