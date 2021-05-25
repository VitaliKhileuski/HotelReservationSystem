using System.Collections.Generic;
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
using HotelReservation.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class UsersService : IUserService
    {
        private readonly UserRepository _userRepository;
        private readonly Mapper _mapper;
        private readonly ILogger<UsersService> _logger;
        private readonly HashPassword _hash;
        private readonly ITokenService _tokenService;
        public UsersService(ILogger<UsersService> logger, UserRepository userRepository, MapConfiguration cfg, HashPassword hash, ITokenService tokenService)
        {
            _mapper = new Mapper(cfg.UserConfiguration);
            _userRepository = userRepository;
            _logger = logger;
            _hash = hash;
            _tokenService = tokenService;
        }

        public ICollection<UserModel>  GetAll()
        {
            var users = _userRepository.GetAll();
            if (!users.Any())
            {
                _logger.LogError("no data about users");
                throw new NotFoundException("no data about users");
            }
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

        public void Update(int userId,UserModel user)
        {
            var userEntity = _userRepository.Get(userId);
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
            _userRepository.Update(userEntity);
        }
    }
}