using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Exceptions;
using Business.Mappers;
using Business.Models;
using HotelReservation.Data.Repositories;

namespace Business.Services
{
    public class UsersService
    {
        private readonly UserRepository _userRepository;
        private readonly UserModelsMapper _userMapper;
        public UsersService(UserRepository userRepository, UserModelsMapper userMapper)
        {
            _userRepository = userRepository;
            _userMapper = userMapper;
        }

        public List<UserModel>  GetAll()
        {
            return  _userMapper.FromEntitiesToResponseModels(_userRepository.GetAll().ToList());
        }

        public UserModel GetById(int id)
        {
            return _userMapper.FromEntityToResponseModel(_userRepository.Get(id));
        }

        public void DeleteById(int id)
        {
            _userRepository.Delete(id);
        }
        public void Update(int id,UserModel user)
        {
            var dbUser = _userRepository.Get(id);
            if (dbUser == null)
            {
                throw new NotFoundException("user with that id not found");
            }
            var newUser =  _userMapper.FromRequestToEntityModel(user);
            dbUser.Email = newUser.Email;
            dbUser.Name = newUser.Name;
            dbUser.Surname = newUser.Surname;
            dbUser.Birthdate = dbUser.Birthdate;
            dbUser.PhoneNumber = dbUser.PhoneNumber;
            _userRepository.Update(dbUser);
        }

        public void Add(UserModel user)
        {
           var userEntity = _userMapper.FromRequestToEntityModel(user);
            _userRepository.Create(userEntity);
        }


    }
}
