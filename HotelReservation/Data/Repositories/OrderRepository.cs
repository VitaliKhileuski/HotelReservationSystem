using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class OrderRepository : BaseRepository<OrderEntity> , IOrderRepository
    {
        private readonly Context _db;

        public OrderRepository(Context context) : base(context)
        {
            _db = context;
        }
    }
}