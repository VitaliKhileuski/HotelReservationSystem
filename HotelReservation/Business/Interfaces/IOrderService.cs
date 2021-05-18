using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IOrderService
    {
        public Task<OrderModel> GetOrderById(int orderId);
        public ICollection<OrderModel> GetAll();
        public Task CreateOrder(int roomId, int userId, OrderModel order);
        public Task UpdateOrder(int orderId, OrderModel newOrder);
        public Task DeleteOrder(int orderId);
    }
}