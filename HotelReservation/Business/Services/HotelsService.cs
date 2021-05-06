using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private readonly Mapper _locationMapper;
        private readonly Mapper _hotelMapper;

        public HotelsService(HotelRepository hotelRepository,LocationRepository locationRepository,MapConfiguration cfg)
        {
            _hotelRepository = hotelRepository;
            _locationRepository = locationRepository;
            _locationMapper = new Mapper(cfg.LocationConfiguration);
            _hotelMapper = new Mapper(cfg.HotelConfiguration);
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
    }
}
