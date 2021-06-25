using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Models;
using HotelReservation.Api.Helpers;
using HotelReservation.Api.Models.RequestModels;
using HotelReservation.Api.Models.ResponseModels;

namespace HotelReservation.Api.Mappers
{
    public class HotelConfiguration : MapperConfiguration
    {
        public HotelConfiguration() : base(x =>
        {
            x.CreateMap<HotelModel, HotelResponseModel>()
                .ForMember(x => x.ImageUrls, opt => opt.MapFrom(hotel => UrlHelper.GetUrls(hotel.Attachments)));
            x.CreateMap<HotelRequestModel, HotelModel>();
            x.CreateMap<LocationRequestModel, LocationModel>();
            x.CreateMap<LocationModel, LocationResponseModel>();
            x.CreateMap<RoomModel, RoomResponseModel>();
            x.CreateMap<ServiceModel, ServiceResponseModel>();
            x.CreateMap<UserModel, UserResponseViewModel>();
        })
        {

        }
    }
}
