﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Repositories;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Business.Services
{ 
    public class HotelsService : IHotelsService
    {
        private readonly HotelRepository _hotelRepository;
        private readonly LocationRepository _locationRepository;
        private readonly UserRepository _userRepository;
        private readonly Mapper _locationMapper;
        private readonly Mapper _hotelMapper;
        private readonly Mapper _roomMapper;

        public HotelsService(HotelRepository hotelRepository,LocationRepository locationRepository,UserRepository userRepository, MapConfiguration cfg)
        {
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _locationRepository = locationRepository;
            _locationMapper = new Mapper(cfg.LocationConfiguration);
            _hotelMapper = new Mapper(cfg.HotelConfiguration);
            _roomMapper = new Mapper(cfg.RoomConfiguration);
        }

        public async Task AddHotel(HotelModel hotel)
        {
            var hotelEntity = _hotelMapper.Map<HotelModel,HotelEntity>(hotel);
            var locationEntity = _locationMapper.Map<LocationModel,LocationEntity>(hotel.Location);
            hotelEntity.Location = locationEntity;
            locationEntity.Hotel = hotelEntity;
            await _hotelRepository.CreateAsync(hotelEntity);
        }

        public async Task<HotelModel> GetById(int id)
        {
            var hotelModel = _hotelMapper.Map<HotelEntity,HotelModel>(await _hotelRepository.GetAsync(id));
            return hotelModel;

        }
        public List<HotelModel> GetAll()
        {
            var hotelModels = _hotelMapper.Map<List<HotelModel>>(_hotelRepository.GetAll().ToList());
            return hotelModels;
        }

        public void  UpdateHotelAdmin(int hotelId, int userId)
        {
            var hotelEntity = _hotelRepository.Get(hotelId);
            var userEntity = _userRepository.Get(userId);
            hotelEntity.HotelAdminId = userEntity.Id;
            userEntity.RoleId = 3;
            _hotelRepository.Update(hotelEntity);
            _userRepository.Update(userEntity);
        }

        public async Task UpdateHotel(int hotelId, HotelModel hotel,int userId)
        {
            var userEntity = await _userRepository.GetAsync(userId);
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            var newHotelEntity = _hotelMapper.Map<HotelModel, HotelEntity>(hotel);
            if (newHotelEntity.Location != null)
            {
                hotelEntity.Location = newHotelEntity.Location;
            }

            if (newHotelEntity.Name != null)
            {
                hotelEntity.Name = newHotelEntity.Name;
            }
            if (hotelEntity.HotelAdminId == userId || userEntity.RoleId==1)
            {
                
                await _hotelRepository.UpdateAsync(hotelEntity);

            }
            else
            {
                throw new BadRequestException("you don't have permission to edit this hotel");
            }
        }

        public async Task AddRoom(int hotelId, RoomModel room, int userId)
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
    }
}
