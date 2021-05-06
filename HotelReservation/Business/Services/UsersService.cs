using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Exceptions;
using Business.Mappers;
using Business.Models;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Repositories;

namespace Business.Services
{
    public class UsersService
    {
        private readonly UserRepository _userRepository;
        private readonly Mapper _mapper;
        public UsersService(UserRepository userRepository, MapConfiguration cfg)
        {
            _mapper = new Mapper(cfg.UserConfiguration);
            _userRepository = userRepository;
        }

        public List<UserModel>  GetAll()
        {
            return  _mapper.Map<List<UserModel>>(_userRepository.GetAll().ToList());
        }

        public UserModel GetById(int id)
        {
            return _mapper.Map<UserEntity,UserModel>(_userRepository.Get(id));
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
            var newUser =  _mapper.Map<UserModel,UserEntity>(user);
            dbUser.Email = newUser.Email;
            dbUser.Name = newUser.Name;
            dbUser.Surname = newUser.Surname;
            dbUser.Birthdate = dbUser.Birthdate;
            dbUser.PhoneNumber = dbUser.PhoneNumber;
            _userRepository.Update(dbUser);
        }

        public void Add(UserModel user)
        {
           var userEntity = _mapper.Map<UserModel,UserEntity>(user);
            _userRepository.Create(userEntity);
        }


    }
}
