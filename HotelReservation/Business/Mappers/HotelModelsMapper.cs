using System;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using AutoMapper;
using Business.Models.RequestModels;
using Business.Models.ResponseModels;
using HotelReservation.Data.Entities;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;

namespace Business.Mappers
{
    public class HotelModelsMapper
    {
        private readonly Mapper _mapper;

        public HotelModelsMapper()
        {
            _mapper = new Mapper(new MapperConfiguration(x => x.CreateMap<HotelRequestModel, HotelEntity>()
                .ForMember(x => x.Location, opt => opt.Ignore())));
        }

        public HotelEntity FromRequestToEntityModel(HotelRequestModel requestModel)
        {
            return  _mapper.Map<HotelRequestModel, HotelEntity>(requestModel);

        }
    }
}
