using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using AutoMapper;
using Business.Mappers;
using Business.Models;
using HotelReservation.Api.Helpers;
using HotelReservation.Api.Models.RequestModels;
using HotelReservation.Api.Models.ResponseModels;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using HotelReservation.Data.Repositories;

namespace HotelReservation.Api.Mappers
{
    public class OrderConfiguration : MapperConfiguration
    {
        public OrderConfiguration() : base(x =>
        {
            x.CreateMap<OrderRequestModel, OrderModel>();
            x.CreateMap<ServiceQuantityRequestModel, ServiceQuantityModel>();
            x.CreateMap<ServiceQuantityModel, ServiceQuantityResponseModel>();
            x.CreateMap<ServiceRequestModel, ServiceModel>()
                .ForMember(x => x.Hotel, opt => opt.Ignore());
            x.CreateMap<ServiceModel, ServiceResponseModel>();
            x.CreateMap<OrderModel, OrderResponseModel>();
            x.CreateMap<RoleModel, RoleResponseModel>();
            x.CreateMap<RoomModel, RoomResponseModel>()
                .ForMember(x => x.ImageUrls, opt => opt.MapFrom(room => UrlHelper.GetUrls(room.Attachments))); ;
            x.CreateMap<HotelModel, HotelResponseModel>()
                .ForMember(X => X.Services, opt => opt.Ignore());
            x.CreateMap<LocationModel, LocationResponseModel>();
            x.CreateMap<UserModel, UserResponseViewModel>()
                .ForMember(x => x.Orders, opt => opt.Ignore());
        })
        {

        }
       
    }

}
