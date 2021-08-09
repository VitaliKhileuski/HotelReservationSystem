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
using Business.Models.FilterModels;
using HotelReservation.Data.Constants;
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
            if (PermissionVerifier.CheckHotelPermission(hotelEntity, userEntity))
            {
                hotelEntity.Rooms.Add(roomEntity); 
                await _hotelRepository.UpdateAsync(hotelEntity);
            }
        }

        public async Task UpdateRoom(Guid roomId, string userId,RoomModel room)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            var userEntity =await _userRepository.GetAsync(userId);
            var hotelEntity = roomEntity.Hotel; 
            if (PermissionVerifier.CheckHotelPermission(hotelEntity, userEntity))
            {
                roomEntity.RoomNumber = room.RoomNumber;
                roomEntity.BedsNumber = room.BedsNumber;
                roomEntity.PaymentPerDay = room.PaymentPerDay;
                await _roomRepository.UpdateAsync(roomEntity);
            }
        }
        public async Task<PageInfo<RoomModel>> GetRoomsPage(Guid hotelId,RoomFilter roomFilter, Pagination roomPagination,SortModel sortModel)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }

            var roomNumber = roomFilter.RoomNumber;
            var checkInDate = roomFilter.CheckInDate;
            var checkOutDate = roomFilter.CheckOutDate;

            var userEntity = await _userRepository.GetAsync(roomFilter.UserId);

            var filteredRooms = new List<RoomEntity>();
            if (userEntity!=null && userEntity.Role.Name!=Roles.User)
            {
                filteredRooms.AddRange(_roomRepository.GetFilteredRooms(hotelEntity,roomNumber,sortModel.SortField,sortModel.Ascending));
                return  PageInfoCreator<RoomModel>.GetPageInfo(_roomMapper.Map<ICollection<RoomModel>>(filteredRooms), roomPagination);

            }
            if (hotelEntity.Rooms != null)
            {
                foreach (var room in hotelEntity.Rooms)
                {
                    
                    if (room.UnblockDate==null || room.PotentialCustomerId==roomFilter.UserId ||  DateTime.Now > room.UnblockDate)
                    {

                        if (room.Orders != null && room.Orders.Count != 0)
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
            if (PermissionVerifier.CheckHotelPermission(hotelEntity, userEntity))
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

        private static bool IsAvailableToBook(RoomEntity room, DateTime checkInDate, DateTime checkOutDate)
        {
            var orderEntity = room.Orders.FirstOrDefault(
                order => order.StartDate.Date >= checkInDate.Date && order.StartDate.Date < checkOutDate.Date ||
                         order.EndDate.Date > checkInDate.Date && order.EndDate.Date <= checkOutDate.Date);
            return orderEntity == null;
        }

        public async Task<bool> IsPossibleToShiftCheckOutTime(Guid roomId, DateTime checkOutDate)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            if (roomEntity == null)
            {
                _logger.LogError($"room with {roomId} id not exists");
                throw new NotFoundException($"room with {roomId} id not exists");
            }

            if (roomEntity.Orders == null || roomEntity.Orders.Count == 0)
            {
                return true;
            }

            foreach (var order in roomEntity.Orders)
            {
                if (checkOutDate.Date < order.StartDate.Date)
                {
                    return false;
                }
            }

            return true;
        }

        public async Task BlockRoomById(Guid roomId,string userId)
        {
            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }
            var roomEntity = await _roomRepository.GetAsync(roomId);
            if (roomEntity == null)
            {
                _logger.LogError($"room with {roomId} id not exists");
                throw new NotFoundException($"room with {roomId} id not exists");
            }
            const double blockTime = 30;
            var unblockDate = DateTime.Now.AddMinutes(blockTime);
            roomEntity.UnblockDate = unblockDate;
            roomEntity.PotentialCustomerId = userId;
                await _roomRepository.UpdateAsync(roomEntity);
        }

        public async Task<bool> IsRoomBlocked(Guid roomId,string userId)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            if (roomEntity.UnblockDate == null || roomEntity.PotentialCustomerId==userId ||  roomEntity.UnblockDate<DateTime.Now)
            {
                return false;
            }


            return true;
        }

        public async Task<LimitHoursModel> GetLimitHours(Guid roomId)
        {
            var roomEntity = await _roomRepository.GetAsync(roomId);
            if (roomEntity == null)
            {
                _logger.LogError($"room with {roomId} id not exists");
                throw new NotFoundException($"room with {roomId} id not exists");
            }

            var roomLimitHours = new LimitHoursModel()
            {
                CheckInTime = roomEntity.Hotel.CheckInTime,
                CheckOutTime = roomEntity.Hotel.CheckOutTime
            };
            return roomLimitHours;
        }
    }
}