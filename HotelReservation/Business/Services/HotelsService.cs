using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly HotelModelsMapper _hotelMapper;
        private readonly LocationModelsMapper _locationMapper;

        public HotelsService(HotelRepository hotelRepository,LocationRepository locationRepository,HotelModelsMapper hotelMapper,LocationModelsMapper locationMapper)
        {
            _hotelRepository = hotelRepository;
            _locationRepository = locationRepository;
            _hotelMapper = hotelMapper;
            _locationMapper = locationMapper;
        }

        public async Task AddHotel(HotelModel hotel)
        {
            var hotelEntity = _hotelMapper.FromRequestToEntityModel(hotel);
            var locationEntity = _locationMapper.FromRequestToEntityModel(hotel.Location);
            hotelEntity.Location = locationEntity;
            locationEntity.Hotel = hotelEntity;
            await _hotelRepository.CreateAsync(hotelEntity);
        }

        public async Task<HotelModel> GetById(int id)
        {
            var hotelModel = _hotelMapper.FromEntityToModel(await _hotelRepository.GetAsync(id));
            return hotelModel;

        }
    }
}
