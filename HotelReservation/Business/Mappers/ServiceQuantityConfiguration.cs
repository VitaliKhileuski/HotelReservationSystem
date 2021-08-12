using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
    public class ServiceQuantityConfiguration : MapperConfiguration
    {
        public ServiceQuantityConfiguration() : base(x =>
        {
            x.CreateMap<ServiceQuantityModel, ServiceQuantityEntity>();
            x.CreateMap<ServiceModel, ServiceEntity>();
            x.CreateMap<OrderModel, OrderEntity>();
        })
        {

        }
    }
}
