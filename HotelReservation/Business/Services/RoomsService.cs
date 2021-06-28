﻿using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ICollection<RoomModel>> GetRoomsFromHotel(Guid hotelId)
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
        public async Task<PageInfo<RoomModel>> GetRoomsPage(Guid hotelId,Pagination hotelPagination)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }
            var rooms = _roomMapper.Map<IEnumerable<RoomModel>>(_roomRepository.GetRoomsPageFromHotel(hotelPagination.PageNumber,
                hotelPagination.PageSize,hotelId));
            var numberOfRooms = await _roomRepository.GetRoomsCount(hotelId);

            int  numberOfPages = numberOfRooms / hotelPagination.PageSize;
            if (numberOfRooms % hotelPagination.PageSize != 0)
            {
                numberOfPages++;
            }
            var roomPageInfo = new PageInfo<RoomModel>
            {
                Items = rooms, NumberOfItems = numberOfRooms, NumberOfPages = numberOfPages
            };


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
    }
}