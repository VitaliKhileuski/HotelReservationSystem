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
    public class HotelsService : IHotelsService
    {
        private readonly  IBaseRepository<HotelEntity> _hotelRepository;
        private readonly IUserRepository _userRepository;
        private readonly Mapper _locationMapper;
        private readonly Mapper _hotelMapper;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<HotelsService> _logger;

        public HotelsService(ILogger<HotelsService>  logger, IBaseRepository<HotelEntity> hotelRepository,IRoleRepository roleRepository, IUserRepository userRepository, MapConfiguration cfg)
        {
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _locationMapper = new Mapper(cfg.LocationConfiguration);
            _hotelMapper = new Mapper(cfg.HotelConfiguration);
            _logger = logger;
            _roleRepository = roleRepository;

        }

        public async Task AddHotel(HotelModel hotel,int hotelAdminId)
        {
            if (hotel == null)
            {
                _logger.LogError("incorrect input data");
                throw new BadRequestException("incorrect input data");
            }
            var hotelEntity = _hotelMapper.Map<HotelModel, HotelEntity>(hotel);
            var userEntity = await _userRepository.GetAsync(hotelAdminId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {hotelAdminId} id not exists");
                throw new NotFoundException($"user with {hotelAdminId} id not exists");
            }
            
            hotelEntity.Admin = userEntity;
            var locationEntity = _locationMapper.Map<LocationModel, LocationEntity>(hotel.Location);
            var roleEntity = await _roleRepository.GetAsyncByName(Roles.HotelAdmin);
            userEntity.Role = roleEntity;
            hotelEntity.Location = locationEntity;
            locationEntity.Hotel = hotelEntity;

            await _userRepository.UpdateAsync(userEntity);
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

            return hotelModels;
        }

        public async Task UpdateHotelAdmin(int hotelId, int userId)
        {
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
            hotelEntity.Admin = userEntity;
            var roleEntity = await _roleRepository.GetAsyncByName(Roles.HotelAdmin);
            userEntity.RoleId = roleEntity.Id;
           await _hotelRepository.UpdateAsync(hotelEntity);
           await _userRepository.UpdateAsync(userEntity);
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

            if (hotelEntity.Admin.Id == userId || userEntity.Role.Name == Roles.Admin)
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

                await _hotelRepository.UpdateAsync(hotelEntity);
            }
            else
            {
                _logger.LogError("you don't have permission to edit this hotel");
                throw new BadRequestException("you don't have permission to edit this hotel");
            }
        }
        public async Task DeleteHotelById(int hotelId)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }
            await _hotelRepository.DeleteAsync(hotelId);
        }

        public async Task<Tuple<IEnumerable<HotelModel>,int>> GetHotelsPage(HotelPagination hotelPagination)
        {
            var hotels = _hotelMapper.Map<IEnumerable<HotelModel>>(_hotelRepository.Find(hotelPagination.PageNumber,hotelPagination.PageSize));
            var numberOfPages = await  _hotelRepository.GetCountAsync();


            return Tuple.Create(hotels,numberOfPages);
        }

    public Tuple<List<HotelModel>,int> GetFilteredHotels(DateTime checkInDate,DateTime checkOutDate,string country,string city, HotelPagination hotelPagination)
        {
            int pages=0;
            var filteredHotels = new List<HotelModel>();
            bool flag = false;
            var hotels = GetAll();
            if (country == "null")
            {
                country = null;//fix
            }
            if (city == "null")
            {
                city = null;//fix
            }
            foreach (var hotel in hotels)
            {
                if (string.IsNullOrEmpty(country) || hotel.Location.Country == country)
                {
                    if (string.IsNullOrEmpty(city) || hotel.Location.City == city)
                    {
                        if (hotel.Rooms == null)//fix
                        {
                            foreach (var room in hotel.Rooms)
                            {
                                if (room.Orders != null)
                                {
                                    if (room.Orders.Any(order => !(checkInDate > order.StartDate && checkInDate < order.EndDate || checkOutDate > order.StartDate && checkOutDate < order.EndDate
                                                                   || order.StartDate > checkInDate && order.StartDate < checkOutDate || order.EndDate > checkInDate && order.EndDate < checkOutDate)))
                                    {
                                        filteredHotels.Add(hotel);
                                        flag = true;
                                    }

                                    if (flag) break;
                                }
                                else
                                {
                                    filteredHotels.Add(hotel);
                                }
                            }
                        
                        }
                        else
                        {
                            filteredHotels.Add(hotel);
                        }
                    }
                }
            }
            pages = filteredHotels.Count / hotelPagination.PageSize;
            if (filteredHotels.Count % hotelPagination.PageSize != 0)
            {
                pages++;
            }
            var pagedData = filteredHotels
                .Skip((hotelPagination.PageNumber - 1) * hotelPagination.PageSize)
                .Take(hotelPagination.PageSize)
                .ToList();

            return Tuple.Create(pagedData, pages);
        }
    }
}