using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using HotelReservation.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class FacilitiesService : IFacilityService
    {
        private readonly IBaseRepository<HotelEntity> _hotelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly Mapper _mapper;
        private readonly ILogger<FacilitiesService> _logger;

        public FacilitiesService(ILogger<FacilitiesService> logger, IBaseRepository<HotelEntity> hotelRepository, IUserRepository userRepository,
            IServiceRepository serviceRepository, MapConfiguration cfg)
        {
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _serviceRepository = serviceRepository;
            _mapper = new Mapper(cfg.ServiceConfiguration);
            _logger = logger;
        }



        public ICollection<ServiceModel> GetAllServices()
        {
            var services = _mapper.Map<ICollection<ServiceModel>>(_serviceRepository.GetAll());
            return services;
        }

        public async Task<ServiceModel> GetServiceById(int serviceId)
        {
            var service = await _serviceRepository.GetAsync(serviceId);
            if (service == null)
            {
                _logger.LogError($"service with {serviceId} id not exists");
                throw new NotFoundException($"service with {serviceId} id not exists");
            }

            return _mapper.Map<ServiceEntity, ServiceModel>(service);

        }
        public async Task<Tuple<IEnumerable<ServiceModel>, int>> GetServicesPage(int hotelId, HotelPagination hotelPagination)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }
            var services = _mapper.Map<IEnumerable<ServiceModel>>(_serviceRepository.GetServicesPageFromHotel(hotelPagination.PageNumber,
                hotelPagination.PageSize, hotelId));
            var numberOfServices = await _serviceRepository.GetServiceCount(hotelId);


            return Tuple.Create(services, numberOfServices);
        }

        public async Task UpdateService(int serviceId, int userId, ServiceModel serviceModel)
        {
            var serviceEntity = await _serviceRepository.GetAsync(serviceId);
            if (serviceEntity == null)
            {
                _logger.LogError($"service with {serviceId} id not exists");
                throw new NotFoundException($"service with {serviceId} id not exists");
            }

            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

            if (serviceEntity.Hotel.Admins.FirstOrDefault(x => x.Id == userId) != null || userEntity.Role.Name == Roles.Admin)
            {
                serviceEntity.Name = serviceModel.Name;
                serviceEntity.Payment = serviceModel.Payment;
                await _serviceRepository.UpdateAsync(serviceEntity);
            }
            else
            {
                _logger.LogError("you don't have permission to delete this service");
                throw new BadRequestException("you don't have permission to delete this service");
            }
        }

        public async Task AddServiceToHotel(int hotelId, int userId, ServiceModel serviceModel)
        {
            var userEntity = await _userRepository.GetAsync(userId);
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);

            if (hotelEntity.Admins.FirstOrDefault(x => x.Id==userId) != null || userEntity.Role.Name==Roles.Admin)
            {
                if (hotelEntity == null)
                {
                    _logger.LogError($"hotel with {hotelId} id doesn't exists");
                    throw new NotFoundException($"hotel with {hotelId} id doesn't exists");
                }

                var service = hotelEntity.Services.FirstOrDefault(x => x.Name == serviceModel.Name);
                if (service != null)
                {
                    throw new BadRequestException("Service with that name already exists");
                }

                var serviceEntity = _mapper.Map<ServiceModel, ServiceEntity>(serviceModel);
                hotelEntity.Services.Add(serviceEntity);
                serviceEntity.Hotel = hotelEntity; 
               await _hotelRepository.UpdateAsync(hotelEntity);
            }
            else
            {
                _logger.LogError("you don't have permission to edit this hotel");
                throw new BadRequestException("you don't have permission to edit this hotel");
            }
        }

        public async Task DeleteOrderFromHotel(int serviceId, int userId)
        {
            var serviceEntity = await _serviceRepository.GetAsync(serviceId);
            var userEntity = await _userRepository.GetAsync(userId);
            var hotelEntity = serviceEntity.Hotel;
            if (hotelEntity.Admins.FirstOrDefault(x => x.Id == userId) != null || userEntity.Role.Name == Roles.Admin)
            {
               await _serviceRepository.DeleteAsync(serviceId);
            }
            else
            {
                _logger.LogError("you don't have permission to delete this service");
                throw new BadRequestException("you don't have permission to delete this service");
            }
        }
    }
}