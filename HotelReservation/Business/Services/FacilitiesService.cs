using System;
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
    public class FacilitiesService : IFacilityService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IUserRepository _userRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly Mapper _mapper;
        private readonly ILogger<FacilitiesService> _logger;

        public FacilitiesService(ILogger<FacilitiesService> logger, IHotelRepository hotelRepository, IUserRepository userRepository,
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

        public async Task<ServiceModel> GetServiceById(Guid serviceId)
        {
            var service = await _serviceRepository.GetAsync(serviceId);
            if (service == null)
            {
                _logger.LogError($"service with {serviceId} id not exists");
                throw new NotFoundException($"service with {serviceId} id not exists");
            }

            return _mapper.Map<ServiceEntity, ServiceModel>(service);

        }
        public async Task<Tuple<IEnumerable<ServiceModel>, int>> GetServicesPage(Guid hotelId, Pagination hotelPagination)
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

        public async Task UpdateService(Guid serviceId, string userId, ServiceModel serviceModel)
        {
            var serviceEntity = await _serviceRepository.GetAsync(serviceId);
            if (serviceEntity == null)
            {
                _logger.LogError($"service with {serviceId} id not exists");
                throw new NotFoundException($"service with {serviceId} id not exists");
            }
            if (serviceEntity.Name != serviceModel.Name)
            {
                var service = serviceEntity.Hotel.Services.FirstOrDefault(x => x.Name == serviceModel.Name);
                if (service != null)
                {
                    throw new BadRequestException("Service with that name already exists");
                }
            }
            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

            if (PermissionVerifier.CheckHotelPermission(serviceEntity.Hotel, userEntity))
            {
                serviceEntity.Name = serviceModel.Name;
                serviceEntity.Payment = serviceModel.Payment;
                await _serviceRepository.UpdateAsync(serviceEntity);
            }
        }

        public async Task AddServiceToHotel(Guid hotelId, string userId, ServiceModel serviceModel)
        {
            var userEntity = await _userRepository.GetAsync(userId);
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);

            if (PermissionVerifier.CheckHotelPermission(hotelEntity, userEntity))
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
        }

        public async Task DeleteOrderFromHotel(Guid serviceId, string userId)
        {
            var serviceEntity = await _serviceRepository.GetAsync(serviceId);
            var userEntity = await _userRepository.GetAsync(userId);
            var hotelEntity = serviceEntity.Hotel;
            if (PermissionVerifier.CheckHotelPermission(hotelEntity, userEntity))
            {
               await _serviceRepository.DeleteAsync(serviceId);
            }
        }
    }
}