using System;
using System.Collections.Generic;
using System.Linq;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    class OrderRepository : IRepository<OrderEntity>
    {
        private Context db;

        public OrderRepository(Context context)
        {
            this.db = context;
        }
        public void Create(OrderEntity order)
        {
            db.Orders.Add(order);
        }

        public void Delete(int id)
        {
            OrderEntity order = db.Orders.Find(id);
            if (order != null)
            {
                db.Orders.Remove(order);
            }
        }

        public IEnumerable<OrderEntity> Find(Func<OrderEntity, bool> predicate)
        {
            return db.Orders.Include(x => x.Customer).Include(x => x.Hotel)
                .Where(predicate).ToList();
        }

        public OrderEntity Get(int id)
        {
            return db.Orders.Find(id);
        }

        public IEnumerable<OrderEntity> GetAll()
        {
            return db.Orders.Include(x => x.Customer).Include(x => x.Hotel);
        }

        public void Update(OrderEntity order)
        {
            db.Entry(order);
        }
    }
}
