using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace HotelReservation.Api.Mappers
{
    public class RequestMapper
    {
        public  M  MapItem<R, M>(R  item)
        {
            var configuration = new MapperConfiguration(x =>
            {
                x.CreateMap<R, M>();
            });
            var mapper = configuration.CreateMapper();
            return mapper.Map<R, M>(item);
        }
        public  List<M> MapItems<R, M>(List<R> items)
        {
            var configuration = new MapperConfiguration(x =>
            {
                x.CreateMap<R, M>();
            });
            var mapper = configuration.CreateMapper();
            return mapper.Map<List<M>>(items);
        }
    }
}