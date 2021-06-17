﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
using HotelReservation.Data;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using HotelReservation.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class UsersService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly Mapper _mapper;
        private readonly ILogger<UsersService> _logger;
        private readonly IPasswordHasher _hash;
        private readonly ITokenService _tokenService;
        public UsersService(ILogger<UsersService> logger, IUserRepository userRepository,
            IHotelRepository hotelRepository, MapConfiguration cfg, IPasswordHasher hash, ITokenService tokenService)
        {
            _mapper = new Mapper(cfg.UserConfiguration);
            _userRepository = userRepository;
            _hotelRepository = hotelRepository;
            _logger = logger;
            _hash = hash;
            _tokenService = tokenService;
        }

        public async Task<ICollection<UserModel>> GetAll(int hotelId)
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

        public async Task<UserModel> GetById(int userId)
        {
            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }
            return _mapper.Map<UserEntity,UserModel>(userEntity);
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
            userEntity.Password = _hash.GenerateHash(userEntity.Password, SHA256.Create());
            userEntity.RoleId = user.RoleId;

            var refreshToken = new RefreshTokenEntity
            {
                Token = _tokenService.GenerateRefreshToken(),
                User = userEntity
            };
            userEntity.RefreshToken = refreshToken;
            await _userRepository.CreateAsync(userEntity);
        }

        public async Task DeleteById(int userId)
        {

            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }

            await _userRepository.DeleteAsync(userId);
        }

        public async Task Update(int userId,UserModel user)
        {
            var userEntity = await _userRepository.GetAsync(userId);
            if (userEntity == null)
            {
                _logger.LogError($"user with {userId} id not exists");
                throw new NotFoundException($"user with {userId} id not exists");
            }
            var newUser = _mapper.Map<UserModel,UserEntity>(user);
            userEntity.Email = newUser.Email;
            userEntity.Name = newUser.Name;
            userEntity.Surname = newUser.Surname;
            userEntity.PhoneNumber = userEntity.PhoneNumber;
            await _userRepository.UpdateAsync(userEntity);
        }
    }
}