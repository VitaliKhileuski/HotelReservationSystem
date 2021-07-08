using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace Business.Mappers
{
    public class UserConfiguration : MapperConfiguration
    {
        public UserConfiguration() : base(x =>
        {
            x.CreateMap<UserEntity, UserModel>().ReverseMap();
            x.CreateMap<RegisterUserModel, UserEntity>().ReverseMap();
            x.CreateMap<RoleEntity, RoleModel>();
            x.CreateMap<OrderEntity, OrderModel>();
            x.CreateMap<RoomEntity, RoomModel>()
                .ForMember(x => x.Orders, opt => opt.Ignore());
            x.CreateMap<ServiceEntity, ServiceModel>()
                .ForMember(x => x.Rooms, opt => opt.Ignore());
            x.CreateMap<HotelEntity, HotelModel>();
            x.CreateMap<LocationEntity, LocationModel>();
            x.CreateMap<AttachmentEntity, AttachmentModel>();
            x.CreateMap<FileContentEntity, FileContentModel>();
        })
        {

        }
    }
}
