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
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class RoomsService : IRoomService
    {
        private readonly RoomRepository _roomRepository;
        private readonly HotelRepository _hotelRepository;
        private readonly UserRepository _userRepository;
        private readonly Mapper _roomMapper;
        private readonly ILogger<RoomsService> _logger;

        public RoomsService(ILogger<RoomsService> logger, RoomRepository roomRepository, HotelRepository hotelRepository,UserRepository userRepository,MapConfiguration cfg)
        {
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
            _roomMapper = new Mapper(cfg.RoomConfiguration);
            _logger = logger;
        }

        public async Task AddRoom(int hotelId,RoomModel room,int userId)
        {
            if (room == null)
            {
                _logger.LogError("incorrect input data");
                throw new BadRequestException("incorrect input data");
            }

            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }

            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }
            var roomEntity = _roomMapper.Map<RoomModel, RoomEntity>(room);
            roomEntity.IsEmpty = true;
            if (hotelEntity.HotelAdminId == userId || userEntity.Role.Name == "Admin")
            {
                hotelEntity.Rooms.Add(roomEntity); 
                _hotelRepository.Update(hotelEntity);
            }
            else
            {
                _logger.LogError("you don't have permission to edit this hotel");
                throw new BadRequestException("you don't have permission to edit this hotel");
            }
        }

        public async Task<ICollection<RoomModel>> GetAllRooms()
        {
            var roomEntities = await _roomRepository.GetAllAsync();
            if (!roomEntities.Any())
            {
                _logger.LogError("no data about rooms in this hotel");
                throw new NotFoundException("no data about rooms in this hotel");
            }

            var roomModels = _roomMapper.Map<ICollection<RoomModel>>(roomEntities);
            return roomModels.ToList();
        }

        public async Task<ICollection<RoomModel>> GetRoomsFromHotel(int hotelId)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }

            if (hotelEntity.Rooms.Capacity == 0)
            {
                _logger.LogError("no rooms in this hotel");
                throw new NotFoundException("no rooms in this hotel");
            }

            return _roomMapper.Map<ICollection<RoomModel>>(hotelEntity.Rooms.ToList());
        }

        public async Task UpdateRoom(int roomId, int userId,RoomModel room)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            var userEntity =await _userRepository.GetAsync(userId);
            var hotelEntity = roomEntity.Hotel; 
            if (hotelEntity.HotelAdminId == userId || userEntity.Role.Name=="Admin")
            {
                roomEntity.BedsNumber = room.BedsNumber;
                roomEntity.PaymentPerDay = room.PaymentPerDay;
                _roomRepository.Update(roomEntity);
            }
            else
            {
                _logger.LogError("you don't have permission to edit this hotel");
                throw new BadRequestException("you don't have permission to edit this hotel");
            }
        }

        public async Task DeleteRoom(int roomId, int userId)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            if (roomEntity == null)
            {
                _logger.LogError($"room with {roomId} id not exists");
                throw new NotFoundException($"room with {roomId} id not exists");
            }

            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }
            var hotelEntity = roomEntity.Hotel;
            if (hotelEntity.HotelAdminId == userId || userEntity.Role.Name=="Admin")
            {
                await _roomRepository.DeleteAsync(roomId);
            }
            else
            {
                _logger.LogError("you don't have permission to edit this hotel");
                throw new BadRequestException("you don't have permission to edit this hotel");
            }
        }
    }
}