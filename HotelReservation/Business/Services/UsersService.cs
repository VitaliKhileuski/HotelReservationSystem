using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Interfaces;
using Business.Mappers;
using Business.Models;
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
        public UsersService(ILogger<UsersService> logger, UserRepository userRepository, MapConfiguration cfg)
        {
            _mapper = new Mapper(cfg.UserConfiguration);
            _userRepository = userRepository;
            _logger = logger;
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
            userEntity.Birthdate = userEntity.Birthdate;
            userEntity.PhoneNumber = userEntity.PhoneNumber;
            _userRepository.Update(userEntity);
        }
    }
}