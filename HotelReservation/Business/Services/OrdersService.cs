using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
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

        public async Task CreateOrder(Guid roomId, string userId, OrderModel order)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            if (roomEntity == null)
            {
                _logger.LogError($"room with {roomId} id not exists");
                throw new NotFoundException($"room with {roomId} id not exists");
            }
            var orderEntity = _mapper.Map<OrderModel, OrderEntity>(order);
            List<ServiceEntity> services = new List<ServiceEntity>();
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
            orderEntity.Services = services;
            var userEntity = await _userRepository.GetAsync(userId);
            
            orderEntity.Customer = userEntity;
            roomEntity.User = userEntity;
            orderEntity.DateOrdered = DateTime.Now;
            orderEntity.NumberOfDays = orderEntity.EndDate.Subtract(orderEntity.StartDate).Days;
            orderEntity.FullPrice = GetFullPrice(orderEntity,roomEntity);
            orderEntity.Room = roomEntity;
            userEntity.Orders.Add(orderEntity);
            await _orderRepository.CreateAsync(orderEntity);
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
            currentOrder.Services = services;
            await  _orderRepository.UpdateAsync(currentOrder);
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

        private decimal GetFullPrice(OrderEntity order,RoomEntity room)
        {
            return order.EndDate.Subtract(order.StartDate).Days * room.PaymentPerDay + order.Services.Sum(service => service.Payment);
        }
    }
}