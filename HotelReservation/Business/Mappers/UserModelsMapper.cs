using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
   public class UserModelsMapper
    {
        private readonly Mapper _mapper;
        private readonly Mapper _newmapper;

        public UserModelsMapper()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.CreateMap<UserEntity, UserModel>()));
            _newmapper = new Mapper(new MapperConfiguration(x => x.CreateMap<UserModel,UserEntity>()));
        }

        public UserEntity FromRequestToEntityModel(UserModel requestModel)
        {
            return _newmapper.Map<UserModel, UserEntity>(requestModel);
        }

        public UserModel FromEntityToResponseModel(UserEntity userEntity)
        {
            return _mapper.Map<UserEntity, UserModel>(userEntity);
        }

        public List<UserModel> FromEntitiesToResponseModels(List<UserEntity> usersEntity)
        {
             return _mapper.Map<List<UserModel>>(usersEntity);
        }
    }
}
