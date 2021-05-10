using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Mappers;
using Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Repositories;

namespace Business.Services
{
    public class OrdersService
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

        public async Task CreateOrder(int roomId, int userId,OrderModel order)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            var userEntity = await _userRepository.GetAsync(userId);
            var orderEntity = _mapper.Map<OrderModel, OrderEntity>(order);
            orderEntity.Customer = userEntity;
            orderEntity.DateOrdered = DateTime.Now;
            orderEntity.NumberOfDays = orderEntity.EndDate.Subtract(orderEntity.StartDate).Days;
            orderEntity.FullPrice = orderEntity.NumberOfDays * roomEntity.PaymentPerDay;
            orderEntity.Room = roomEntity;
            await _orderRepository.CreateAsync(orderEntity);
        }
    }
}
