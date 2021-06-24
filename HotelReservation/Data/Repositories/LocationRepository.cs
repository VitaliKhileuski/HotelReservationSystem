using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
   public  class LocationRepository : BaseRepository<LocationEntity> , ILocationRepository
    {
        private readonly Context _db;

        public LocationRepository(Context context) :base(context)
        {
            _db = context;
        }
    }
}