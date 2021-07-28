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
using Business.Models.FilterModels;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
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
        private readonly IFileContentRepository _fileContentRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<HotelsService> _logger;

        public HotelsService(ILogger<HotelsService>  logger, IHotelRepository hotelRepository,IRoleRepository roleRepository,
            IUserRepository userRepository,IFileContentRepository fileContentRepository, MapConfiguration cfg)
        {
            _hotelRepository = hotelRepository;
            _userRepository = userRepository;
            _fileContentRepository = fileContentRepository;
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

        public async Task<HotelModel> GetById(Guid hotelId)
        {
            var hotelModel = _hotelMapper.Map<HotelEntity, HotelModel>(await _hotelRepository.GetAsync(hotelId));
            if (hotelModel == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }
            return hotelModel;

        }

        public async Task UpdateHotelAdmin(Guid hotelId,Guid adminId)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }

            var userEntity = await _userRepository.GetAsync(adminId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {adminId} id not exists");
                throw new NotFoundException($"user with {adminId} id not exists");
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

        public async Task UpdateHotel(Guid hotelId, HotelModel hotel,string userId)
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

            

            if (PermissionVerifier.CheckHotelPermission(hotelEntity, userEntity))
            {

                hotelEntity.Name = hotel.Name;
                hotelEntity.Location.Country = hotel.Location.Country;
                hotelEntity.Location.City = hotel.Location.City;
                hotelEntity.Location.Street = hotel.Location.Street;
                hotelEntity.Location.BuildingNumber = hotel.Location.BuildingNumber;

                    await _hotelRepository.UpdateAsync(hotelEntity);
            }
        }
        public async Task DeleteHotelById(Guid hotelId,string userId)
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
            if (PermissionVerifier.CheckHotelPermission(hotelEntity, userEntity))
            {
                var imageIds = hotelEntity.Attachments.Select(image => image.FileContent.Id).ToList();

                foreach (var id in imageIds)
                {
                    await _fileContentRepository.DeleteAsync(id);
                }
                await _hotelRepository.DeleteAsync(hotelId);
            }
        }

        public async Task<ICollection<UserModel>> GetHotelAdmins(Guid hotelId)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }

            return _userMapper.Map<ICollection<UserModel>>(hotelEntity.Admins);
        }

        public async Task<PageInfo<HotelModel>> GetFilteredHotels(HotelFilterModel hotelFilter, Pagination hotelPagination,SortModel sortModel)
        {

            bool flag = false;
            var userId = hotelFilter.UserId;
            var country = hotelFilter.Country;
            var city = hotelFilter.City;
            var checkInDate = hotelFilter.CheckInDate;
            var checkOutDate = hotelFilter.CheckOutDate;
            var hotelName = hotelFilter.HotelName;
            var email = hotelFilter.Email;
            var surname = hotelFilter.Surname;
            var adminEntity = await _userRepository.GetAsync(userId);
            var availableHotels = new List<HotelEntity>();

            var filteredHotels = _hotelRepository.GetFilteredHotels(country, city, hotelName, email, surname,sortModel.SortField,sortModel.Ascending);
            if (adminEntity != null && adminEntity.Role.Name==Roles.User || adminEntity==null)
            {
                foreach (var hotel in filteredHotels)
                {
                    var a = hotel.Rooms;
                    
                    if (hotel.Rooms != null)
                    {
                        foreach (var room in hotel.Rooms.Where(room => room.UnblockDate == null || room.PotentialCustomerId == userId || DateTime.Now > room.UnblockDate))
                        {
                            if (room.Orders != null && room.Orders.Count != 0)
                            {
                                if (room.Orders.All(order => !(checkInDate > order.StartDate && checkInDate < order.EndDate ||
                                                               checkOutDate > order.StartDate && checkOutDate < order.EndDate ||
                                                               order.StartDate > checkInDate && order.StartDate < checkOutDate ||
                                                               order.EndDate > checkInDate && order.EndDate < checkOutDate)))
                                {
                                    availableHotels.Add(hotel);
                                    flag = true;
                                }

                                if (flag) break;
                            }
                            else
                            {
                                availableHotels.Add(hotel);
                                break;
                            }
                        }
                    }
                    else
                    {
                        availableHotels.Add(hotel);
                    }
                }
                var hotelModels = _hotelMapper.Map<ICollection<HotelModel>>(availableHotels);
                var hotelPageInfo = PageInfoCreator<HotelModel>.GetPageInfo(hotelModels, hotelPagination);
                return hotelPageInfo;
            }
            else
            {
                var hotelModels = _hotelMapper.Map<ICollection<HotelModel>>(filteredHotels);
                var hotelPageInfo = PageInfoCreator<HotelModel>.GetPageInfo(hotelModels, hotelPagination);
                return hotelPageInfo;
            }
        }

        public async Task<PageInfo<HotelModel>> GetHotelsPageForHotelAdmin(HotelFilterModel hotelFilter, Pagination hotelPagination, SortModel sortModel)
        {
            var hotelAdmin = await _userRepository.GetAsync(hotelFilter.UserId);
            if (hotelAdmin == null)
            {
                _logger.LogError($"hotel admin with {hotelFilter.UserId} id not exists");
                throw new NotFoundException($"hotel admin with {hotelFilter.UserId} id not exists");
            }
            hotelFilter.Email = hotelAdmin.Email;

            var filteredHotels = _hotelRepository.GetFilteredHotels(hotelFilter.Country, hotelFilter.City, hotelFilter.HotelName,
                hotelFilter.Email, hotelFilter.Surname, sortModel.SortField, sortModel.Ascending);
            var hotelModels = _hotelMapper.Map<ICollection<HotelModel>>(filteredHotels);
            var hotelPageInfo = PageInfoCreator<HotelModel>.GetPageInfo(hotelModels, hotelPagination);
            return hotelPageInfo;
        }

        public async Task DeleteHotelAdmin(Guid hotelId, Guid userId)
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

        public async Task<IEnumerable<string>> GetHotelRoomsNumbers(Guid hotelId, string userId)
        {
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

            if(PermissionVerifier.CheckHotelPermission(hotelEntity, userEntity))
            {
                var roomsNumbers = hotelEntity.Rooms.Select(x => x.RoomNumber);
                return roomsNumbers;
            }

            throw new BadRequestException("you don't have permissions to do this action");
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

        public IEnumerable<string> GetHotelNames()
        {
            return _hotelRepository.GetHotelNames();
        }
    }
}