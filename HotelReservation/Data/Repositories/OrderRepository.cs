using System.Collections.Generic;
using System.Linq;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Helpers;
using HotelReservation.Data.Interfaces;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace HotelReservation.Data.Repositories
{
    public class OrderRepository : BaseRepository<OrderEntity> , IOrderRepository
    {
        private readonly Context _db;

        public OrderRepository(Context context) : base(context)
        {
            _db = context;
        }

        public IEnumerable<OrderEntity> GetFilteredOrders(UserEntity user, string country, string city, string surname,string orderNumber,string sortField,bool ascending)
        {
            IQueryable<OrderEntity> orderItems = _db.Orders;
            if (user != null)
            {
                if (user.Role.Name == Roles.HotelAdmin)
                {
                    orderItems = user.OwnedHotels.SelectMany(x => x.Rooms).SelectMany(x => x.Orders).AsQueryable();
                }
                else if (user.Role.Name == Roles.User)
                {
                    orderItems = user.Orders.AsQueryable();
                }
            }
            var orderEntities = orderItems.Where(x => 
                                                      (!string.IsNullOrEmpty(country) && x.Room.Hotel.Location.Country==country || string.IsNullOrEmpty(country)) &&
                                                      (!string.IsNullOrEmpty(city) && x.Room.Hotel.Location.City==city || string.IsNullOrEmpty(city)) &&
                                                      (!string.IsNullOrEmpty(surname) && x.Customer.Surname==surname || string.IsNullOrEmpty(surname)) &&
                                                      (!string.IsNullOrEmpty(orderNumber) && x.Number.StartsWith(orderNumber) || string.IsNullOrEmpty(orderNumber)) );
            if (string.IsNullOrEmpty(sortField))
            {
                return orderEntities;
            }

            return orderEntities.OrderByPropertyName(sortField, ascending);
        }
    }
}