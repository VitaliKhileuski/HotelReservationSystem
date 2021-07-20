using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Helpers;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
using Castle.Core.Internal;
using HotelReservation.Data.Constants;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class UsersService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly Mapper _mapper;
        private readonly ILogger<UsersService> _logger;
        private readonly IPasswordHasher _hash;
        private readonly ITokenService _tokenService;
        public UsersService(ILogger<UsersService> logger, IUserRepository userRepository,
            IHotelRepository hotelRepository,IRoleRepository roleRepository, MapConfiguration cfg, IPasswordHasher hash, ITokenService tokenService)
        {
            _mapper = new Mapper(cfg.UserConfiguration);
            _userRepository = userRepository;
            _hotelRepository = hotelRepository;
            _roleRepository = roleRepository;
            _logger = logger;
            _hash = hash;
            _tokenService = tokenService;
        }

        public async Task<ICollection<UserModel>> GetAll(Guid hotelId)
        {
            var hotelEntity = await _hotelRepository.GetAsync(hotelId);
            if (hotelEntity == null)
            {
                _logger.LogError($"hotel with {hotelId} id not exists");
                throw new NotFoundException($"hotel with {hotelId} id not exists");
            }

            var users = _userRepository.GetUsers(hotelEntity);
            return  _mapper.Map<ICollection<UserModel>>(users);
        }

        public async Task<UserModel> GetById(Guid id,string userId)
        {
            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

            if (PermissionVerifier.CheckUserPermission(userEntity, userId))
            {
                return _mapper.Map<UserEntity, UserModel>(userEntity);
            }

            throw new BadRequestException("you don't have permissions to get user info");
        }

        public async Task AddUser(UserModel user)
        {
            if (user == null)
            {
                _logger.LogError("incorrect input data");
                throw new BadRequestException("incorrect input data");
            }

            var dbUser = await _userRepository.GetAsyncByEmail(user.Email);
            if (dbUser != null)
            {
                _logger.LogError("user with that email already exists");
                throw new BadRequestException("user with that email already exists");
            }

            var userEntity = _mapper.Map<UserModel, UserEntity>(user);
            var password = user.Password.IsNullOrEmpty() ? "testtest1" : user.Password;
            userEntity.Password = _hash.GenerateHash(password, SHA256.Create());
            var role = await _roleRepository.GetAsyncByName(Roles.User);
            userEntity.Role = role;
            var refreshToken = new RefreshTokenEntity
            {
                Token = _tokenService.GenerateRefreshToken(),
                User = userEntity
            };
            userEntity.RefreshToken = refreshToken;
            await _userRepository.CreateAsync(userEntity);
        }

        public async Task DeleteById(Guid userId)
        {

            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

            await _userRepository.DeleteAsync(userId);
        }

        public async Task<PageInfo<UserModel>> GetUsersPage(string email,string surname, string userId, Pagination pagination)
        {
            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

            if (email == "null")
            {
                email = null;
            }

            if (surname == "null")
            {
                surname = null;
            }
            ICollection<UserModel> filteredUsers = new List<UserModel>();
            if (surname != null)
            {
                filteredUsers = _mapper.Map<ICollection<UserModel>>(_userRepository.GetFilteredUsersBySurname(surname));
            }
            else if (email != null)
            {
                filteredUsers.Add(_mapper.Map<UserEntity,UserModel>(await _userRepository.GetAsyncByEmail(email)));
            }
            else
            {
                filteredUsers = _mapper.Map<ICollection<UserModel>>(_userRepository.GetAll());
            }
            var page = PageInfoCreator<UserModel>.GetPageInfo(filteredUsers, pagination);
            return page;
        }

        public async Task<string> Update(Guid id,string userId, UserModel user)
        {
            var userEntity = await _userRepository.GetAsync(id);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

            if (PermissionVerifier.CheckUserPermission(userEntity, userId))
            {
                var newUser = _mapper.Map<UserModel, UserEntity>(user);

                if (newUser.Email != null && userEntity.Email!=user.Email)
                {
                    var dbUser = await _userRepository.GetAsyncByEmail(user.Email);
                    if (dbUser != null)
                    {
                        _logger.LogError("user with that email already exists");
                        throw new BadRequestException("user with that email already exists");
                    }
                    userEntity.Email = newUser.Email;
                }

                if (newUser.Name != null)
                {
                    userEntity.Name = newUser.Name;
                }

                if (newUser.Surname != null)
                {
                    userEntity.Surname = newUser.Surname;
                }

                if (newUser.PhoneNumber != null)
                {
                    userEntity.PhoneNumber = newUser.PhoneNumber;
                }

                if (newUser.Password != null)
                {
                    userEntity.Password = _hash.GenerateHash(newUser.Password, SHA256.Create());
                }

                await _userRepository.UpdateAsync(userEntity);
                var newToken = _tokenService.BuildToken(userEntity.Email, userEntity.Role.Name, userEntity.Name,
                    userEntity.Id);
                return newToken;
            }

            throw new BadRequestException("you don't have any permissions");
        }

        public IEnumerable<string> GetUsersEmails()
        {
          var emails = _userRepository.GetUsersEmails();
          return emails;
        }
        public IEnumerable<string> GetUsersSurnames()
        {
            var surnames = _userRepository.GetUsersSurnames();
            return surnames;
        }

        public IEnumerable<string> GetHotelAdminsEmails()
        {
            var emails = _userRepository.GetHotelAdminsEmails();
            return emails;
        }

        public IEnumerable<string> GetHotelAdminsSurnames()
        {
            var surnames = _userRepository.GetHotelAdminsSurnames();
            return surnames;
        }

        public IEnumerable<string> GetCustomersSurnames()
        {
            var surnames = _userRepository.GetCustomersSurnames();
            return surnames;
        }
    }
}