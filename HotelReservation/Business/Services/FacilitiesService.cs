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
    public class FacilitiesService
    {
        private readonly HotelRepository _hotelRepository;
        private readonly RoomRepository _roomRepository;
        private readonly ServiceRepository _serviceRepository;
        private readonly UserRepository _userRepository;
        private readonly Mapper _mapper;

        public FacilitiesService(HotelRepository hotelRepository, RoomRepository roomRepository,
            ServiceRepository serviceRepository, UserRepository userRepository, MapConfiguration cfg)
        {
            _hotelRepository = hotelRepository;
            _roomRepository = roomRepository;
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _mapper = new Mapper(cfg.ServiceConfiguration);
        }

        public async Task AddServiceToHotel(int hotelId, int userId, ServiceModel serviceModel)
        {
            var userEntity = await _userRepository.GetAsync(userId);
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);

            if (hotelEntity.HotelAdminId == userId || userEntity.RoleId == 1)
            {
                if (hotelEntity == null)
                {
                    throw new NotFoundException("that hotel doesn't exists");
                }

                var service = hotelEntity.Services.FirstOrDefault(x => x.Name == serviceModel.Name);
                if (service != null)
                {
                    throw new BadRequestException("Service with that name already exists");
                }

                var serviceEntity = _mapper.Map<ServiceModel, ServiceEntity>(serviceModel);
                hotelEntity.Services.Add(serviceEntity);
                serviceEntity.Hotel = hotelEntity; 
                _hotelRepository.Update(hotelEntity);
            }
            else
            {
                throw new BadRequestException("you don't have permission to edit this hotel");
            }
        }

    }
}
