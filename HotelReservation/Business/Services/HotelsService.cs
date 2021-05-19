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
using HotelReservation.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class HotelsService : IHotelsService
    {
        private readonly HotelRepository _hotelRepository;
        private readonly UserRepository _userRepository;
        private readonly Mapper _locationMapper;
        private readonly Mapper _hotelMapper;
        private readonly ILogger<HotelsService> _logger;

        public HotelsService(ILogger<HotelsService>  logger, HotelRepository hotelRepository, UserRepository userRepository, MapConfiguration cfg)
        {
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _locationMapper = new Mapper(cfg.LocationConfiguration);
            _hotelMapper = new Mapper(cfg.HotelConfiguration);
            _logger = logger;
            
        }

        public async Task AddHotel(HotelModel hotel)
        {
            if (hotel == null)
            {
                _logger.LogError("incorrect input data");
                throw new BadRequestException("incorrect input data");
            }
            var hotelEntity = _hotelMapper.Map<HotelModel, HotelEntity>(hotel);
            var locationEntity = _locationMapper.Map<LocationModel, LocationEntity>(hotel.Location);
            hotelEntity.Location = locationEntity;
            locationEntity.Hotel = hotelEntity;
            await _hotelRepository.CreateAsync(hotelEntity);
        }

        public async Task<HotelModel> GetById(int hotelId)
        {
            var hotelModel = _hotelMapper.Map<HotelEntity, HotelModel>(await _hotelRepository.GetAsync(hotelId));
            if (hotelModel == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }
            return hotelModel;

        }

        public List<HotelModel> GetAll()
        {
            var hotelModels = _hotelMapper.Map<List<HotelModel>>(_hotelRepository.GetAll().ToList());
            if (hotelModels.Count == 0)
            {
                _logger.LogError("no data about hotels");
                throw new NotFoundException("no data about hotels");
            }
            return hotelModels;
        }

        public void UpdateHotelAdmin(int hotelId, int userId)
        {
            var hotelEntity = _hotelRepository.Get(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }
            var userEntity = _userRepository.Get(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }
            hotelEntity.HotelAdminId = userEntity.Id;
            userEntity.RoleId = 3;
            _hotelRepository.Update(hotelEntity);
            _userRepository.Update(userEntity);
        }

        public async Task UpdateHotel(int hotelId, HotelModel hotel, int userId)
        {
            if (hotel == null)
            {
                _logger.LogError("incorrect input data");
                throw new BadRequestException("incorrect input data");
            }

            var userEntity = await _userRepository.GetAsync(userId);

            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

            var hotelEntity = await _hotelRepository.GetAsync(hotelId);

            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }

            if (hotelEntity.HotelAdminId == userId || userEntity.Role.Name == "Admin")
            {
                var newHotelEntity = _hotelMapper.Map<HotelModel, HotelEntity>(hotel);

                if (newHotelEntity.Location != null)
                {
                    hotelEntity.Location = newHotelEntity.Location;
                }

                if (newHotelEntity.Name != null)
                {
                    hotelEntity.Name = newHotelEntity.Name;
                }

                _hotelRepository.Update(hotelEntity);
            }
            else
            {
                _logger.LogError("you don't have permission to edit this hotel");
                throw new BadRequestException("you don't have permission to edit this hotel");
            }
        }
        public async Task DeleteHotelById(int hotelId)
        {
            var hotelEntity = _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }
            await _hotelRepository.DeleteAsync(hotelId);
        }
    }
}