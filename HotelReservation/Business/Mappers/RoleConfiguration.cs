using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
    public class RoleConfiguration : MapperConfiguration
    {
        public RoleConfiguration() : base(x =>
        {
            x.CreateMap<RoleEntity, RoleModel>().ReverseMap();
            x.CreateMap<UserEntity, UserModel>()
                .ForMember(x => x.Role, opt => opt.Ignore());
        })
        {

        }
    }
}
