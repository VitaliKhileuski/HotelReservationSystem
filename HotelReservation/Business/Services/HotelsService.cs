using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Interfaces;
using Business.Mappers;
using Business.Models.RequestModels;
using HotelReservation.Data.Repositories;

namespace Business.Services
{ 
    public class HotelsService : IHotelsService
    {
        private readonly HotelRepository _hotelRepository;
        private readonly HotelModelsMapper _hotelMapper;

        public HotelsService(HotelRepository hotelRepository,HotelModelsMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _hotelMapper = mapper;
        }

        public async Task AddHotel(HotelRequestModel hotel)
        {
            var hotelEntity = _hotelMapper.FromRequestToEntityModel(hotel);
            await _hotelRepository.CreateAsync(hotelEntity);
        }
    }
}
