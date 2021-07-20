using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Helpers;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class OrdersService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly Mapper _mapper;
        private readonly ILogger<OrdersService> _logger;
        public OrdersService(ILogger<OrdersService> logger, IOrderRepository orderRepository, IUserRepository userRepository,
            IRoomRepository roomRepository,IServiceRepository serviceRepository, MapConfiguration cfg)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _mapper = new Mapper(cfg.OrderConfiguration);
            _serviceRepository = serviceRepository;
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

        public async Task CreateOrder(Guid roomId, OrderModel order)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            if (roomEntity == null)
            {
                _logger.LogError($"room with {roomId} id not exists");
                throw new NotFoundException($"room with {roomId} id not exists");
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
            orderEntity.NumberOfDays = orderEntity.EndDate.Subtract(orderEntity.StartDate).Days;
            orderEntity.FullPrice = GetFullPrice(orderEntity,roomEntity);
            orderEntity.Room = roomEntity;
            userEntity.Orders.Add(orderEntity);
            roomEntity.Orders.Add(orderEntity);
            await _userRepository.UpdateAsync(userEntity);
            await _roomRepository.UpdateAsync(roomEntity);
        }

        public async Task UpdateOrder(Guid orderId, OrderModel newOrder)
        {
            if (newOrder == null)
            {
                _logger.LogError("incorrect input data");
                throw new BadRequestException("incorrect input data");
            }
            var currentOrder = await _orderRepository.GetAsync(orderId);
            var orderEntity = _mapper.Map<OrderModel, OrderEntity>(newOrder);
            if (currentOrder == null)
            {
                _logger.LogError($"order with {orderId} id not exists");
                throw new NotFoundException($"order with {orderId} id not exists");
            }
            var roomEntity = currentOrder.Room;
            var services = new List<ServiceEntity>();
            foreach (var service in roomEntity.Hotel.Services)
            {
                foreach (var orderService in orderEntity.Services)
                {
                    if (service.Id == orderService.Id)
                    {
                        services.Add(service);
                    }
                }
            }

            orderEntity.Services = null;
            currentOrder.EndDate = orderEntity.EndDate;
            currentOrder.StartDate = orderEntity.StartDate;
            currentOrder.NumberOfDays = currentOrder.EndDate.Subtract(currentOrder.StartDate).Days;
            currentOrder.Services = orderEntity.Services;
            currentOrder.FullPrice = GetFullPrice(currentOrder,roomEntity);
            await _orderRepository.UpdateAsync(currentOrder);
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

        public async Task<PageInfo<OrderModel>> GetOrdersPage(string userId, string country, string city, string surname, Pagination pagination)
        {

            if (country == "null")
            {
                country = null;
            }

            if (city == "null")
            {
                city = null;
            }

            if (surname == "null")
            {
                surname = null;
            }

            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

            return userEntity.Role.Name switch
            {
                Roles.Admin => GetOrdersForAdmin(country,city,surname,pagination),
                Roles.HotelAdmin => GetOrdersForHotelAdmin(userEntity, pagination),
                Roles.User => GetOrdersForUser(userEntity, pagination),
                _ => new PageInfo<OrderModel>()
            };
        }

        private PageInfo<OrderModel> GetOrdersForAdmin(string country,string city,string surname,Pagination pagination)
        { 
            var orderModels = _mapper.Map<ICollection<OrderModel>>(_orderRepository.GetFilteredOrders(country,city,surname));
            var page = PageInfoCreator<OrderModel>.GetPageInfo(orderModels,pagination);
            return page;
        }

        private PageInfo<OrderModel> GetOrdersForHotelAdmin(UserEntity userEntity, Pagination pagination)
        {
            var orders = new List<OrderEntity>();
            foreach (var hotel in userEntity.OwnedHotels)
            {
                foreach (var room in hotel.Rooms)
                {
                    orders.AddRange(room.Orders);
                }
            }

            var orderModels = _mapper.Map<ICollection<OrderModel>>(orders);
            var page = PageInfoCreator<OrderModel>.GetPageInfo(orderModels, pagination);
            return page;
        }

        private PageInfo<OrderModel> GetOrdersForUser(UserEntity userEntity, Pagination pagination)
        {
            var orderModels = _mapper.Map<ICollection<OrderModel>>(userEntity.Orders);
            var page = PageInfoCreator<OrderModel>.GetPageInfo(orderModels, pagination);
            return page;
        }

        private decimal GetFullPrice(OrderEntity order, RoomEntity room)
        {
              return order.EndDate.Subtract(order.StartDate).Days * room.PaymentPerDay + order.Services.Sum(serviceQuantity => serviceQuantity.Service.Payment*serviceQuantity.Quantity);
        }

    }
}