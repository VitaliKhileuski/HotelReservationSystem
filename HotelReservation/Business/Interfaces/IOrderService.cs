using System;
using System.Threading.Tasks;
using Business.Models;
using Business.Models.FilterModels;

namespace Business.Interfaces
{
    public interface IOrderService
    {
        public Task<OrderModel> GetOrderById(Guid orderId);
        public Task<string> CreateOrder(Guid roomId, OrderModel order);
        public Task DeleteOrder(Guid orderId);
        public Task<PageInfo<OrderModel>> GetOrdersPage(string userId, OrderFilter orderFilter, Pagination pagination,SortModel sortModel);
        public Task UpdateOrder(Guid orderId,UpdateOrderModel updateOrderModel);
        public Task<ReviewModel> GetReview(Guid orderId);
    }
}