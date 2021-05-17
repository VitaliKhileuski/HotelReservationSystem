﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Repositories;

namespace Business.Services
{
    public class OrdersService : IOrderService
    {
        private readonly OrderRepository _orderRepository;
        private readonly UserRepository _userRepository;
        private readonly RoomRepository _roomRepository;
        private readonly Mapper _mapper;
        public OrdersService(OrderRepository orderRepository, UserRepository userRepository,
            RoomRepository roomRepository, MapConfiguration cfg)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _mapper = new Mapper(cfg.OrderConfiguration);
        }


        public async Task<OrderModel> GetOrderById(int orderId)
        {
            var order =await _orderRepository.GetAsync(orderId);
            if (order == null)
            {
                throw new NotFoundException("order not found");
            }

            var orderModel = _mapper.Map<OrderEntity, OrderModel>(order);

            return orderModel;
        }

        public ICollection<OrderModel> GetAll()
        {
            var orders = _mapper.Map<ICollection<OrderModel>>(_orderRepository.GetAll());
            if (orders.Count == 0)
            {
                throw new NotFoundException("no data about orders");
            }

            return orders;
        }

        public async Task CreateOrder(int roomId, int userId,OrderModel order)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            if (roomEntity == null)
            {
                throw new NotFoundException("thar room is not found");
            }
            if ((bool)!roomEntity.IsEmpty)
            {
                throw new BadRequestException("this room already reserved");
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
            roomEntity.IsEmpty = false;
            roomEntity.User = userEntity;
            orderEntity.DateOrdered = DateTime.Now;
            orderEntity.NumberOfDays = orderEntity.EndDate.Subtract(orderEntity.StartDate).Days;
            orderEntity.FullPrice = GetFullPrice(orderEntity,roomEntity);
            orderEntity.Room = roomEntity;
            userEntity.Orders.Add(orderEntity);
            await _orderRepository.CreateAsync(orderEntity);
             _roomRepository.Update(roomEntity);
        }

        public async Task UpdateOrder(int orderId, OrderModel newOrder)
        {
            if (newOrder == null)
            {
                throw new BadRequestException("incorrect input data");
            }
            var currentOrder = await _orderRepository.GetAsync(orderId);
            var orderEntity = _mapper.Map<OrderModel, OrderEntity>(newOrder);
            if (currentOrder == null)
            {
                throw new NotFoundException("order with that id not exists");
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
            _orderRepository.Update(currentOrder);
            currentOrder.Services = services;
            _orderRepository.Update(currentOrder);

        }

        public async Task DeleteOrder(int orderId)
        {
            var order = await _orderRepository.GetAsync(orderId);
            if (order == null)
            {
                throw new NotFoundException("order with that id not exists");
            }
            await _orderRepository.DeleteAsync(orderId);
        }

        private decimal GetFullPrice(OrderEntity order,RoomEntity room)
        {
            return order.EndDate.Subtract(order.StartDate).Days * room.PaymentPerDay + order.Services.Sum(service => service.Payment);
        }
    }
}