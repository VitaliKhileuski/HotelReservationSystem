using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Helpers;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class RoomsService : IRoomService
    {
        private readonly IRoomRepository  _roomRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileContentRepository _fileContentRepository;
        private readonly Mapper _roomMapper;
        private readonly ILogger<RoomsService> _logger;

        public RoomsService(ILogger<RoomsService> logger, IRoomRepository roomRepository, IHotelRepository hotelRepository,
            IUserRepository userRepository,IFileContentRepository fileContentRepository, MapConfiguration cfg)
        {
            _userRepository = userRepository;
            _roomRepository = roomRepository;
            _hotelRepository = hotelRepository;
            _fileContentRepository = fileContentRepository;
            _roomMapper = new Mapper(cfg.RoomConfiguration);
            _logger = logger;
        }

        public async Task AddRoom(Guid hotelId,RoomModel room, string userId)
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
            if (PermissionVerifier.CheckPermission(hotelEntity, userEntity))
            {
                hotelEntity.Rooms.Add(roomEntity); 
                await _hotelRepository.UpdateAsync(hotelEntity);
            }
        }

        //public async Task<ICollection<RoomModel>> GetRoomsFromHotel(Guid hotelId,DateTime checkInDate,DateTime checkOutDate)
        //{
        //    var hotelEntity = await _hotelRepository.GetAsync(hotelId);
        //    if (hotelEntity == null)
        //    {
        //        _logger.LogError($"hotel with {hotelId} id not exists");
        //        throw new NotFoundException($"hotel with {hotelId} id not exists");
        //    }

           
        //    return _roomMapper.Map<ICollection<RoomModel>>(filteredRooms.ToList());
        //}

        public async Task UpdateRoom(Guid roomId, string userId,RoomModel room)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            var userEntity =await _userRepository.GetAsync(userId);
            var hotelEntity = roomEntity.Hotel; 
            if (PermissionVerifier.CheckPermission(hotelEntity, userEntity))
            {
                roomEntity.RoomNumber = room.RoomNumber;
                roomEntity.BedsNumber = room.BedsNumber;
                roomEntity.PaymentPerDay = room.PaymentPerDay;
                await _roomRepository.UpdateAsync(roomEntity);
            }
        }
        public async Task<PageInfo<RoomModel>> GetRoomsPage(Guid hotelId,DateTime checkInDate,DateTime checkOutDate, Pagination roomPagination)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }

            var filteredRooms = new List<RoomEntity>();
            if (hotelEntity.Rooms != null)
            {
                foreach (var room in hotelEntity.Rooms)
                {
                    if (room.Orders != null && room.Orders.Count!=0)
                    {
                        if (IsAvailableToBook(room, checkInDate, checkOutDate))
                        {
                            filteredRooms.Add(room);
                        }
                    }
                    else
                    {
                        filteredRooms.Add(room);
                    }
                }

            }

            var filteredRoomModels = _roomMapper.Map<ICollection<RoomModel>>(filteredRooms);
           var roomPageInfo = PageInfoCreator<RoomModel>.GetPageInfo(filteredRoomModels, roomPagination);
            return roomPageInfo;
        }

        public async Task DeleteRoom(Guid roomId, string userId)
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
            if (PermissionVerifier.CheckPermission(hotelEntity, userEntity))
            {
                var imageIds = roomEntity.Attachments.Select(image => image.FileContent.Id).ToList();

                foreach (var id in imageIds)
                {
                    await _fileContentRepository.DeleteAsync(id);
                }
                await _roomRepository.DeleteAsync(roomId);
            }
        }

        public async Task<bool> IsRoomEmpty(Guid roomId, DateTime checkInDate, DateTime checkOutDate)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            if (roomEntity == null)
            {
                _logger.LogError($"room with {roomId} id not exists");
                throw new NotFoundException($"room with {roomId} id not exists");
            }
            if (roomEntity.Orders != null && roomEntity.Orders.Count != 0)
            {
                if (IsAvailableToBook(roomEntity,checkInDate,checkOutDate))
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return false;
        }

        private bool IsAvailableToBook(RoomEntity room, DateTime checkInDate, DateTime checkOutDate)
        {
            return room.Orders.All(order => !(checkInDate > order.StartDate && checkInDate < order.EndDate ||
                                              checkOutDate > order.StartDate && checkOutDate < order.EndDate
                                              || order.StartDate > checkInDate && order.StartDate < checkOutDate ||
                                              order.EndDate > checkInDate && order.EndDate < checkOutDate));
        }
    }
}