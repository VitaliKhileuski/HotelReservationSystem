using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Mappers;
using Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Repositories;

namespace Business.Services
{
    public class RoomsService
    {
        private readonly RoomRepository _roomRepository;
        private readonly HotelRepository _hotelRepository;
        private readonly UserRepository _userRepository;
        private readonly Mapper _roomMapper;

        public RoomsService(RoomRepository roomRepository, HotelRepository hotelRepository,UserRepository userRepository,MapConfiguration cfg)
        {
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
            _roomMapper = new Mapper(cfg.RoomConfiguration);
        }

        public async Task AddRoom(int hotelId,RoomModel room,int userId)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            var userEntity = await _userRepository.GetAsync(userId);
            var roomEntity = _roomMapper.Map<RoomModel, RoomEntity>(room);
            if (hotelEntity.HotelAdminId == userId || userEntity.RoleId == 1)
            {
                hotelEntity.Rooms.Add(roomEntity);
                await _hotelRepository.UpdateAsync(hotelEntity);
            }
            else
            {
                throw new BadRequestException("you don't have permission to edit this hotel");
            }
        }

        public async Task<List<RoomModel>> GetAllRooms()
        {
            var roomEntities = await _roomRepository.GetAllAsync();
            if (!roomEntities.Any())
            {
                throw new NotFoundException("no rooms in database");
            }

            var roomModels = _roomMapper.Map<List<RoomModel>>(roomEntities);

            return roomModels.ToList();
        }

        public async Task<List<RoomModel>> GetAllEmptyRooms()
        {
            var emptyRooms = await GetAllRooms();
            return emptyRooms.Where(x => x.IsEmpty).ToList();
        }

        public async Task<List<RoomModel>> GetRoomsFromHotel(int hotelId)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                throw new NotFoundException($"Can't find hotel with id {hotelId}");
            }

            if (hotelEntity.Rooms.Capacity == 0)
            {
                throw new NotFoundException("no room in this hotel");
            }

            return _roomMapper.Map<List<RoomModel>>(hotelEntity.Rooms.ToList());
        }

        public async Task UpdateRoom(int roomId, int userId,RoomModel room)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            var userEntity =await _userRepository.GetAsync(userId);
            var hotelEntity = roomEntity.Hotel; 
            if (hotelEntity.HotelAdminId == userId || userEntity.RoleId == 1)
            {
                roomEntity.BedsNumber = room.BedsNumber;
                roomEntity.MiniBar = room.MiniBar;
                roomEntity.RoomNumber = room.RoomNumber;
                roomEntity.PaymentPerDay = room.PaymentPerDay;
                roomEntity.WiFi = room.WiFi;
                await _roomRepository.UpdateAsync(roomEntity);
            }
            else
            {
                throw new BadRequestException("you don't have permission to edit this hotel");
            }
        }

        public async Task DeleteRoom(int roomId, int userId)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            var userEntity = await _userRepository.GetAsync(userId);
            var hotelEntity = roomEntity.Hotel;
            if (hotelEntity.HotelAdminId == userId || userEntity.RoleId == 1)
            {
                await _roomRepository.DeleteAsync(roomId);
            }
            else
            {
                throw new BadRequestException("you don't have permission to edit this hotel");
            }
        }
    }
}
