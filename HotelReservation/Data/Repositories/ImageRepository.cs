using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class ImageRepository : BaseRepository<ImageEntity> , IImageRepository
    {
        private readonly Context _db;

        public ImageRepository(Context context) : base(context)
        {
            _db = context;
        }
    }
}