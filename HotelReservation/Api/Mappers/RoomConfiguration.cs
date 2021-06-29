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
    public class RoomConfiguration : MapperConfiguration
    {
        public RoomConfiguration() : base(x =>
        {
            x.CreateMap<RoomModel, RoomResponseModel>()
                .ForMember(x => x.Hotel, opt => opt.Ignore())
                .ForMember(x => x.ImageUrls, opt => opt.MapFrom(room => UrlHelper.GetUrls(room.Attachments)));
            x.CreateMap<RoomRequestModel, RoomModel>();
            x.CreateMap<HotelModel, HotelResponseModel>()
                .ForMember(x => x.Services, opt => opt.Ignore())
                .ForMember(x => x.Rooms, opt => opt.Ignore());
            x.CreateMap<LocationModel, LocationResponseModel>();
        })
        {

        }
    }
}
