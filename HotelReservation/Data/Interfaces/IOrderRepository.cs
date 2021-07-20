using System.Collections.Generic;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IOrderRepository : IBaseRepository<OrderEntity>
    {
        public IEnumerable<OrderEntity> GetFilteredOrders(string country, string city, string surname);
    }
}