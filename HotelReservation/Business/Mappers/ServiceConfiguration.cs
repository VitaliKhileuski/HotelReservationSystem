using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
    public class ServiceConfiguration : MapperConfiguration
    {
        public ServiceConfiguration() : base(x =>
        {
            x.CreateMap<ServiceEntity, ServiceModel>()
                .ForMember(x => x.Hotel, opt => opt.Ignore())
                .ForMember(x => x.Rooms, opt => opt.Ignore());
            x.CreateMap<ServiceModel, ServiceEntity>();
        })
        {

        }
    }
}
