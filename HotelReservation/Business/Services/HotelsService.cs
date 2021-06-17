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
        private readonly  IHotelRepository _hotelRepository;
        private readonly IUserRepository _userRepository;
        private readonly Mapper _locationMapper;
        private readonly Mapper _hotelMapper;
        private readonly Mapper _userMapper;
        private readonly IBaseRepository<LocationEntity> _locationRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<HotelsService> _logger;

        public HotelsService(ILogger<HotelsService>  logger, IHotelRepository hotelRepository,IRoleRepository roleRepository,
            IUserRepository userRepository,IBaseRepository<LocationEntity> locationRepository, MapConfiguration cfg)
        {
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _locationRepository = locationRepository;
            _locationMapper = new Mapper(cfg.LocationConfiguration);
            _hotelMapper = new Mapper(cfg.HotelConfiguration);
            _userMapper = new Mapper(cfg.UserConfiguration);
            _logger = logger;
            _roleRepository = roleRepository;

        }

        public async Task AddHotel(HotelModel hotel)
        {
            if (hotel == null)
            {
                _logger.LogError("incorrect input data");
                throw new BadRequestException("incorrect input data");
            }

            var hotelEntity = _hotelMapper.Map<HotelModel, HotelEntity>(hotel);
            if (!IsLocationEmpty(hotelEntity))
            {
                _logger.LogError("the hotel at this location already placed");
                throw new BadRequestException("the hotel at this location already placed");
            }
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

            if (hotelEntity.Admins == null)
            {
                List<UserEntity> admins = new List<UserEntity>
                {
                    userEntity
                };
                hotelEntity.Admins = admins;

            }
            else
            {
                hotelEntity.Admins.Add(userEntity);
            }
            
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
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);

            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }

            var hotelUpdateEntity = _hotelMapper.Map<HotelModel, HotelEntity>(hotel);
            if (!IsLocationEmpty(hotelUpdateEntity, hotelEntity.Location))
            {
                _logger.LogError("the hotel at this location already placed");
                throw new BadRequestException("the hotel at this location already placed");
            }

            var userEntity = await _userRepository.GetAsync(userId);

            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

            

            if (hotelEntity.Admins.FirstOrDefault(x => x.Id == userId) != null || userEntity.Role.Name == Roles.Admin)
            {

                hotelEntity.Name = hotel.Name;
                hotelEntity.Location.Country = hotel.Location.Country;
                hotelEntity.Location.City = hotel.Location.City;
                hotelEntity.Location.Street = hotel.Location.Street;
                hotelEntity.Location.BuildingNumber = hotel.Location.BuildingNumber;

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

        public async Task<Tuple<IEnumerable<HotelModel>,int>> GetHotelsPage(Pagination hotelPagination)
        {
            var hotels = _hotelMapper.Map<IEnumerable<HotelModel>>(_hotelRepository.Find(hotelPagination.PageNumber,hotelPagination.PageSize));
            var numberOfPages = await  _hotelRepository.GetCountAsync();


            return Tuple.Create(hotels,numberOfPages);
        }
        public async Task<Tuple<IEnumerable<HotelModel>, int>> GetHotelAdminPages(Pagination hotelPagination,int hotelAdminId)
        {
            var hotelAdmin = await _userRepository.GetAsync(hotelAdminId);
            if (hotelAdmin == null)
            {
                _logger.LogError($"user with {hotelAdminId} id not exists");
                throw new  NotFoundException($"user with {hotelAdminId} id not exists");
            }

            var hotels = _hotelMapper.Map<IEnumerable<HotelModel>>(_hotelRepository.GetHotelAdminsHotels(hotelPagination.PageNumber, hotelPagination.PageSize,hotelAdmin));
            var numberOfPages = await _hotelRepository.GetHotelAdminsHotelsCount(hotelAdmin);
            return Tuple.Create(hotels, numberOfPages);
        }


        public async Task<ICollection<UserModel>> GetHotelAdmins(int hotelId)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }

            return _userMapper.Map<ICollection<UserModel>>(hotelEntity.Admins);
        }

    public Tuple<List<HotelModel>,int> GetFilteredHotels(DateTime checkInDate,DateTime checkOutDate,string country,string city, Pagination hotelPagination)
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

        public async Task DeleteHotelAdmin(int hotelId, int userId)
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

            hotelEntity.Admins.Remove(userEntity);
            await _hotelRepository.UpdateAsync(hotelEntity);
        }

        public bool IsLocationEmpty(HotelEntity hotel, LocationEntity oldLocation = null)
        {
            
            if (oldLocation!=null && oldLocation.Country == hotel.Location.Country
                && oldLocation.City == hotel.Location.City
                && oldLocation.Street == hotel.Location.Street
                && oldLocation.BuildingNumber == hotel.Location.BuildingNumber)
            {
                return true;
            }
          var hotelEntity = _hotelRepository.GetAll().FirstOrDefault(x => x.Id != hotel.Id  &&
                                                                          x.Location.Country == hotel.Location.Country &&
                                                                          x.Location.City == hotel.Location.City &&
                                                                          x.Location.Street == hotel.Location.Street && x.Location.BuildingNumber == hotel.Location.BuildingNumber);
          return hotelEntity == null;
        }
    }
}