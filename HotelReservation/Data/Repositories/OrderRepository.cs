using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class OrderRepository : IRepository<OrderEntity>
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

        public async Task CreateAsync(OrderEntity order)
        {
            await db.Orders.AddAsync(order);
        }

        public void Delete(int id)
        {
            OrderEntity order = db.Orders.Find(id);
            if (order != null)
            {
                db.Orders.Remove(order);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var order = await db.Orders.FindAsync(id);

            if (order != null)
            {
                db.Orders.Remove(order);
            }
        }

        public IEnumerable<OrderEntity> Find(Func<OrderEntity, bool> predicate)
        {
            return db.Orders.Where(predicate).ToList();
        }

        public  async Task<IEnumerable<OrderEntity>> FindAsync(Func<OrderEntity, bool> predicate)
        {
            return await Task.Run(() => Find(predicate));
        }

        public OrderEntity Get(int id)
        {
            return db.Orders.Find(id);
        }

        public IEnumerable<OrderEntity> GetAll()
        {
            return db.Orders;
        }

        public async Task<IEnumerable<OrderEntity>> GetAllAsync()
        {
            return await Task.Run(GetAll);
        }

        public async Task<OrderEntity> GetAsync(int id)
        {
            return await Task.Run((() => Get(id)));
        }

        public void Update(OrderEntity order)
        {
            db.Entry(order);
        }

        public async Task UpdateAsync(OrderEntity order)
        {
            await Task.Run((() => Update(order)));
        }
    }
}
