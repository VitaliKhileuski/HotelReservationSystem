using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
    public class HotelConfiguration : MapperConfiguration
    {
        public HotelConfiguration() : base(x =>
        {
            x.CreateMap<HotelModel, HotelEntity>();
            x.CreateMap<HotelEntity, HotelModel>();
            x.CreateMap<LocationModel, LocationEntity>();
            x.CreateMap<OrderEntity, OrderModel>();
            x.CreateMap<LocationEntity, LocationModel>()
                .ForMember(x => x.Hotel, opt => opt.Ignore());
            x.CreateMap<RoomEntity, RoomModel>()
                .ForMember(x => x.Hotel, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore());
            x.CreateMap<ServiceQuantityEntity, ServiceQuantityModel>();
            x.CreateMap<ServiceEntity, ServiceModel>();
            x.CreateMap<UserEntity, UserModel>()
                .ForMember(x => x.OwnedHotels, opt => opt.Ignore())
                .ForMember(x => x.Orders, opt => opt.Ignore())
                .ForMember(x => x.Rooms, opt => opt.Ignore())
                .ForMember(x => x.Role, opt => opt.Ignore());
            x.CreateMap<AttachmentEntity, AttachmentModel>().ReverseMap();
            x.CreateMap<FileContentEntity, FileContentModel>().ReverseMap();
        })
        {

        }
    }
}
