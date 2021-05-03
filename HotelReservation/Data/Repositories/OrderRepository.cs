﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;

namespace HotelReservation.Data.Repositories
{
    public class OrderRepository : IRepository<OrderEntity>, IRepositoryAsync<OrderEntity>
    {
        private readonly Context _db;

        public OrderRepository(Context context)
        {
            _db = context;
        }
        public void Create(OrderEntity order)
        {
            _db.Orders.Add(order);
            _db.SaveChanges();
        }

        public async Task CreateAsync(OrderEntity order)
        {
            await _db.Orders.AddAsync(order);
            await _db.SaveChangesAsync();
        }

        public void Delete(int id)
        {
            OrderEntity order = _db.Orders.Find(id);
            if (order != null)
            {
                _db.Orders.Remove(order);
            }

            _db.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var order = await _db.Orders.FindAsync(id);

            if (order != null)
            {
                _db.Orders.Remove(order);
            }

            await _db.SaveChangesAsync();
        }

        public IEnumerable<OrderEntity> Find(Func<OrderEntity, bool> predicate)
        {
            return _db.Orders.Where(predicate).ToList();
        }

        public  async Task<IEnumerable<OrderEntity>> FindAsync(Func<OrderEntity, bool> predicate)
        {
            return await Task.Run(() => Find(predicate));
        }

        public OrderEntity Get(int id)
        {
            return _db.Orders.Find(id);
        }

        public IEnumerable<OrderEntity> GetAll()
        {
            return _db.Orders;
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
            _db.Entry(order);
            _db.SaveChanges();
        }

        public async Task UpdateAsync(OrderEntity order)
        {
            await Task.Run((() => Update(order)));
            await _db.SaveChangesAsync();
        }
    }
}