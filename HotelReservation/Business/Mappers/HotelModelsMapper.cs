using System;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace Business.Mappers
{
    public class HotelModelsMapper
    {
        private readonly Mapper _mapper;

        public HotelModelsMapper()
        {
            _mapper = new Mapper(new MapperConfiguration(x =>
            {
                x.CreateMap<HotelModel, HotelEntity>()
                    .ForMember(x => x.Location, opt => opt.Ignore());
                x.CreateMap<HotelEntity, HotelModel>();
                x.CreateMap<LocationEntity, LocationModel>();
            }));
        }

        public HotelEntity FromRequestToEntityModel(HotelModel requestModel)
        {
            return  _mapper.Map<HotelModel, HotelEntity>(requestModel);

        }

        public HotelModel FromEntityToModel(HotelEntity hotelEntity)
        {
            return _mapper.Map<HotelEntity, HotelModel>(hotelEntity);
        }
    }
}
