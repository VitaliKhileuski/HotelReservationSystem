using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
    public class OrderConfiguration : MapperConfiguration

    {
    public OrderConfiguration() : base(x =>
    {
        x.CreateMap<OrderEntity, OrderModel>().ReverseMap();
        x.CreateMap<ServiceQuantityModel, ServiceQuantityEntity>().ReverseMap();
        x.CreateMap<UserEntity, UserModel>()
            .ForMember(x => x.Orders, opt => opt.Ignore());
        x.CreateMap<RoomEntity, RoomModel>();
        x.CreateMap<ServiceModel, ServiceEntity>().ReverseMap();
        x.CreateMap<RoleEntity, RoleModel>();
        x.CreateMap<HotelEntity, HotelModel>()
            .ForMember(x => x.Reviews, opt => opt.Ignore())
            .ForMember(x => x.AverageCategoryRatings, opt => opt.Ignore());
        x.CreateMap<LocationEntity, LocationModel>();
        x.CreateMap<AttachmentEntity, AttachmentModel>();
        x.CreateMap<FileContentEntity, FileContentModel>();
    })
    {

    }
    }
}
