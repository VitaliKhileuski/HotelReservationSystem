using System.Collections.Generic;
using System.Linq;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Helpers;
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

        public IEnumerable<OrderEntity> GetFilteredOrders(string country, string city, string surname,string sortField,bool ascending)
        {
            var orderEntities = _db.Orders.Where(x => (!string.IsNullOrEmpty(country) && x.Room.Hotel.Location.Country==country || string.IsNullOrEmpty(country)) &&
                                                      (!string.IsNullOrEmpty(city) && x.Room.Hotel.Location.City==city || string.IsNullOrEmpty(city)) &&
                                                      (!string.IsNullOrEmpty(surname) && x.Customer.Surname==surname || string.IsNullOrEmpty(surname)));
            if (string.IsNullOrEmpty(sortField))
            {
                return orderEntities;
            }

            return orderEntities.OrderByPropertyName(sortField, ascending);
        }
    }
}