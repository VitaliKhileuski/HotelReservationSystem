using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
    public class RoomConfiguration : MapperConfiguration
    {
        public RoomConfiguration() : base(x =>
        {
            x.CreateMap<RoomEntity, RoomModel>().ReverseMap();
            x.CreateMap<UserEntity, UserModel>()
                .ForMember(x => x.Rooms, opt => opt.Ignore())
                .ForMember(x => x.Role, opt => opt.Ignore())
                .ForMember(x => x.Orders, opt => opt.Ignore());
            x.CreateMap<HotelEntity, HotelModel>()
                .ForMember(x => x.Rooms, opt => opt.Ignore());
            x.CreateMap<LocationEntity, LocationModel>();
            x.CreateMap<ServiceQuantityEntity, ServiceQuantityModel>();
            x.CreateMap<ServiceEntity, ServiceModel>();
            x.CreateMap<OrderEntity, OrderModel>()
                .ForMember(x => x.Room, opt => opt.Ignore())
                .ForMember(x => x.Customer, opt => opt.Ignore());
            x.CreateMap<AttachmentEntity, AttachmentModel>().ReverseMap();
            x.CreateMap<FileContentEntity, FileContentModel>().ReverseMap();
        })
        {

        }
    }
}
