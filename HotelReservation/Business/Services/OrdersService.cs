using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Helpers;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
using Business.Models.FilterModels;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class OrdersService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly Mapper _mapper;
        private readonly ILogger<OrdersService> _logger;
        public OrdersService(ILogger<OrdersService> logger, IOrderRepository orderRepository, IUserRepository userRepository,
            IRoomRepository roomRepository, MapConfiguration cfg)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _mapper = new Mapper(cfg.OrderConfiguration);
            _logger = logger;
        }

        public async Task<OrderModel> GetOrderById(Guid orderId)
        {
            var order = await _orderRepository.GetAsync(orderId);
            if (order == null)
            {
                _logger.LogError($"order with {orderId} id not found");
                throw new NotFoundException($"order with {orderId} id not found");
            }

            var orderModel = _mapper.Map<OrderEntity, OrderModel>(order);

            return orderModel;
        }

        public async Task<string> CreateOrder(Guid roomId, OrderModel order)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            if (roomEntity == null)
            {
                _logger.LogError($"room with {roomId} id not exists");
                throw new NotFoundException($"room with {roomId} id not exists");
            }
            if(!IsAvailableToBook(roomEntity,order.StartDate,order.EndDate))
            {
                _logger.LogError($"this room is already booked on this dates");
                throw new BadRequestException($"this room is already booked on this dates");
            }

            var orderEntity = _mapper.Map<OrderModel, OrderEntity>(order);
            var serviceQuantities = new List<ServiceQuantityEntity>();
            foreach (var serviceQuantity in order.Services)
            {
                foreach(var service in roomEntity.Hotel.Services)
                {
                    if (service.Id == serviceQuantity.Service.Id)
                    {
                        serviceQuantities.Add(new ServiceQuantityEntity
                        {
                            Service =  service,
                            Quantity = serviceQuantity.Quantity
                        });
                    }
                }
            }

            orderEntity.Services = serviceQuantities;
            var userEntity = await _userRepository.GetAsyncByEmail(order.UserEmail);
            if (userEntity == null)
            {
                _logger.LogError($"user with {order.UserEmail} email not exists");
                throw new NotFoundException($"user with {order.UserEmail} email not exists");
            }

            orderEntity.Customer = userEntity;
            orderEntity.DateOrdered = DateTime.Now;
            orderEntity.NumberOfDays = GetNumberOfDays(orderEntity.StartDate, orderEntity.EndDate);
            orderEntity.FullPrice = GetFullPrice(orderEntity,roomEntity);
            orderEntity.Room = roomEntity;
            roomEntity.UnblockDate = null;
            roomEntity.PotentialCustomerId = null;
            while (true)
            {
                try
                {
                    orderEntity.Number = RandomStringGenerator.GetRandomString(8);
                    userEntity.Orders.Add(orderEntity);
                    roomEntity.Orders.Add(orderEntity);
                    await _userRepository.UpdateAsync(userEntity);
                    await _roomRepository.UpdateAsync(roomEntity);
                    break;
                }
                catch
                {
                    userEntity.Orders.Remove(orderEntity);
                    roomEntity.Orders.Remove(orderEntity);
                }
            }

            return orderEntity.Number;
        }

        public async Task DeleteOrder(Guid orderId)
        {
            var order = await _orderRepository.GetAsync(orderId);
            if (order == null)
            {
                _logger.LogError($"order with {orderId} id not exists");
                throw new NotFoundException($"order with {orderId} id not exists");
            }
            await _orderRepository.DeleteAsync(orderId);
        }

        public async Task<PageInfo<OrderModel>> GetOrdersPage(string userId,OrderFilter orderFilter, Pagination pagination,SortModel sortModel)
        {
            var country = orderFilter.Country;
            var city = orderFilter.City;
            var surname = orderFilter.Surname;
            var orderNumber = orderFilter.Number;
            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

            if (userEntity.Role.Name == Roles.Admin)
            {
                userEntity = null;
            }
           
            var orders = _orderRepository.GetFilteredOrders(userEntity, country, city, surname,orderNumber, sortModel.SortField,
                sortModel.Ascending);

            var orderModels = _mapper.Map<ICollection<OrderModel>>(orders);
            var page = PageInfoCreator<OrderModel>.GetPageInfo(orderModels, pagination);
            return page;

        }

        private static int GetNumberOfDays(DateTime checkInDate, DateTime checkOutDate)
        {
            var numberOfDays = (checkOutDate - checkInDate).Days;
            return numberOfDays;
        }

        private static decimal GetFullPrice(OrderEntity order, RoomEntity room)
        {
              return order.EndDate.Subtract(order.StartDate).Days * room.PaymentPerDay + order.Services.Sum(serviceQuantity => serviceQuantity.Service.Payment*serviceQuantity.Quantity);
        }
        private static bool IsAvailableToBook(RoomEntity room, DateTime checkInDate, DateTime checkOutDate)
        {
            var orderEntity = room.Orders.FirstOrDefault(
                order => order.StartDate >= checkInDate && order.StartDate < checkOutDate ||
                           order.EndDate> checkInDate && order.EndDate <= checkOutDate);
            return orderEntity == null;
        }

        public async Task UpdateOrder(Guid orderId, UpdateOrderModel updateOrderModel)
        {
            var orderEntity = await _orderRepository.GetAsync(orderId);
            if (orderEntity == null)
            {
                _logger.LogError($"order with {orderId} id not exists");
                throw new NotFoundException($"order with {orderId} id not exists");
            }

            if (orderEntity.CheckInTime != updateOrderModel.CheckInTime)
            {
                orderEntity.CheckInTime = updateOrderModel.CheckInTime;
            }

            if (orderEntity.CheckOutTime != updateOrderModel.CheckOutTime)
            {
                orderEntity.CheckOutTime = updateOrderModel.CheckOutTime;
            }

            if (orderEntity.StartDate.Date != updateOrderModel.CheckInDate.Date)
            {
                orderEntity.StartDate = updateOrderModel.CheckInDate;
            }

            if (orderEntity.EndDate.Date != updateOrderModel.CheckOutDate.Date)
            {
                orderEntity.EndDate = updateOrderModel.CheckOutDate;
            }

            orderEntity.FullPrice = GetFullPrice(orderEntity, orderEntity.Room);
            orderEntity.NumberOfDays = GetNumberOfDays(orderEntity.StartDate, orderEntity.EndDate);
            await _orderRepository.UpdateAsync(orderEntity);
        }
    }
}