using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Mappers;
using Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Repositories;
using Microsoft.EntityFrameworkCore.Query;

namespace Business.Services
{
    public class FacilitiesService
    {
        private readonly HotelRepository _hotelRepository;
        private readonly UserRepository _userRepository;
        private readonly ServiceRepository _serviceRepository;
        private readonly Mapper _mapper;

        public FacilitiesService(HotelRepository hotelRepository, UserRepository userRepository,ServiceRepository serviceRepository, MapConfiguration cfg)
        {
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _serviceRepository = serviceRepository;
            _mapper = new Mapper(cfg.ServiceConfiguration);
        }



        public ICollection<ServiceModel> GetAllServices()
        {
            var services = _mapper.Map<ICollection<ServiceModel>>(_serviceRepository.GetAll());
            if (services.Count == 0)
            {
                throw new NotFoundException("no data about services");
            }

            return services;
        }

        public async Task<ServiceModel> GetServiceById(int serviceId)
        {
            var service = await _serviceRepository.GetAsync(serviceId);
            if (service == null)
            {
                throw new NotFoundException("service with that id not exists");
            }

            return _mapper.Map<ServiceEntity, ServiceModel>(service);

        }

        public async Task AddServiceToHotel(int hotelId, int userId, ServiceModel serviceModel)
        {
            var userEntity = await _userRepository.GetAsync(userId);
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);

            if (hotelEntity.HotelAdminId == userId || userEntity.Role.Name=="Admin")
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

        public async Task DeleteOrderFromHotel(int serviceId, int userId)
        {
            var serviceEntity = await _serviceRepository.GetAsync(serviceId);
            var userEntity = await _userRepository.GetAsync(userId);
            var hotelEntity = serviceEntity.Hotel;
            if (hotelEntity.HotelAdminId == userId || userEntity.Role.Name == "Admin")
            {
               await _serviceRepository.DeleteAsync(serviceId);
            }
            else
            {
                throw new BadRequestException("you don't have permission to delete this service");
            }

        }

    }
}
