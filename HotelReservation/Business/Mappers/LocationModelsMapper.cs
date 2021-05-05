using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
  public  class LocationModelsMapper
    {
        private readonly Mapper _mapper;

        public LocationModelsMapper()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.CreateMap<LocationModel, LocationEntity>()));
        }

        public LocationEntity FromRequestToEntityModel(LocationModel requestModel)
        {
            return _mapper.Map<LocationModel,LocationEntity>(requestModel);
        }
    }
}