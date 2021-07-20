﻿using System;
using System.Threading.Tasks;
using Business.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Business.Interfaces
{
    public interface IOrderService
    {
        public Task<OrderModel> GetOrderById(Guid orderId);
        public Task CreateOrder(Guid roomId, OrderModel order);
        public Task UpdateOrder(Guid orderId, OrderModel newOrder);
        public Task DeleteOrder(Guid orderId);
        public Task<PageInfo<OrderModel>> GetOrdersPage(string userId, string country, string city, string surname, Pagination pagination);
    }
}