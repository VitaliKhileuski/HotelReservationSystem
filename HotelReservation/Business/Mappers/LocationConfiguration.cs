using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
    public class LocationConfiguration : MapperConfiguration
    {
        public LocationConfiguration() : base(x =>
        {
            x.CreateMap<LocationModel, LocationEntity>().ReverseMap();
            x.CreateMap<HotelEntity, HotelModel>().ForMember(x => x.Location, opt => opt.Ignore());
        })
        {

        }
    }
}
