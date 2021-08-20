using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
    public class ReviewConfiguration : MapperConfiguration
    {
        public ReviewConfiguration() : base(x =>
        {
            x.CreateMap<ReviewEntity, ReviewModel>()
                .ForMember(x => x.Hotel, opt=> opt.Ignore());
            x.CreateMap<ReviewModel, ReviewEntity>();
            x.CreateMap<ReviewCategoryWithRatingEntity, ReviewCategoryWithRatingModel>().ReverseMap();
            x.CreateMap<ReviewCategoryEntity, ReviewCategoryModel>().ReverseMap();
            x.CreateMap<UserEntity, UserModel>()
                .ForMember(x => x.Orders, opt => opt.Ignore())
                .ForMember(x => x.OwnedHotels, opt => opt.Ignore())
                .ForMember(x => x.Role, opt => opt.Ignore());
            x.CreateMap<OrderEntity, OrderModel>()
                .ForMember(x => x.Services, opt => opt.Ignore())
                .ForMember(x => x.Customer, opt => opt.Ignore());
            x.CreateMap<RoomEntity, RoomModel>()
                .ForMember(x => x.Attachments, opt => opt.Ignore())
                .ForMember(x => x.User, opt => opt.Ignore())
                .ForMember(x => x.Hotel, opt => opt.Ignore())
                .ForMember(x => x.Orders, opt => opt.Ignore());
        })
        {

        }
    }
}
