using AutoMapper;
using Business.Mappers;
using Business.Models;
using HotelReservation.Api.Models.RequestModels;
using HotelReservation.Api.Models.ResponseModels;
using HotelReservation.Data.Entities;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace HotelReservation.Api.Mappers
{
    public class CustomMapperConfiguration
    {
        public MapperConfiguration UsersConfiguration;
        public MapperConfiguration HotelConfiguration;
        public MapperConfiguration RoomConfiguration;
        public MapperConfiguration OrderConfiguration;
        public MapperConfiguration TokenConfiguration;
        public MapperConfiguration ServiceConfiguration;
        public MapperConfiguration UpdateOrderConfiguration;
        public MapperConfiguration ReviewCategoryConfiguration;
        public MapperConfiguration ReviewCategoryWithRatingConfiguration;
        public MapperConfiguration ReviewConfiguration;
        public AttachmentConfiguration AttachmentConfiguration;

        public CustomMapperConfiguration()
        {

            AttachmentConfiguration = new AttachmentConfiguration();
            HotelConfiguration = new  HotelConfiguration();
            OrderConfiguration = new OrderConfiguration();
            RoomConfiguration = new RoomConfiguration();

            UpdateOrderConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<UpdateOrderRequestModel, UpdateOrderModel>();
                x.CreateMap<ServiceQuantityRequestModel, ServiceQuantityModel>();
                x.CreateMap<ServiceRequestModel, ServiceModel>()
                    .ForMember(x => x.Hotel, opt => opt.Ignore());
            });

            ReviewCategoryConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<ReviewCategoryRequestModel, ReviewCategoryModel>();
                x.CreateMap<ReviewCategoryModel, ReviewCategoryResponseModel>();
            });

            ReviewCategoryWithRatingConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<ReviewCategoryWithRatingRequestModel, ReviewCategoryWithRatingModel>();
                x.CreateMap<ReviewCategoryWithRatingModel, ReviewCategoryWithRatingResponseModel>();
                x.CreateMap<ReviewCategoryRequestModel, ReviewCategoryModel>();
                x.CreateMap<ReviewCategoryModel, ReviewCategoryResponseModel>();


            });

            ReviewConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<ReviewRequestModel, ReviewModel>();
                x.CreateMap<ReviewModel, ReviewResponseModel>()
                    .ForMember(x => x.Hotel, opt => opt.Ignore());
                x.CreateMap<ReviewCategoryWithRatingRequestModel, ReviewCategoryWithRatingModel>();
                x.CreateMap<ReviewCategoryWithRatingModel, ReviewCategoryWithRatingResponseModel>();
                x.CreateMap<ReviewCategoryRequestModel, ReviewCategoryModel>();
                x.CreateMap<ReviewCategoryModel, ReviewCategoryResponseModel>();
                x.CreateMap<UserModel, UserResponseViewModel>();
                x.CreateMap<OrderModel, OrderResponseModel>();
                x.CreateMap<RoomModel, RoomResponseModel>();
            });

            UsersConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<UserRequestModel, UserModel>();
                x.CreateMap<LoginUserRequestModel, LoginUserModel>();
                x.CreateMap<RegisterUserRequestModel, RegisterUserModel>();
                x.CreateMap<UserRequestModel, UserModel>();
                x.CreateMap<UserModel, UserResponseViewModel>();
                x.CreateMap<RoleModel, RoleResponseModel>();
                x.CreateMap<OrderModel, OrderResponseModel>();
                x.CreateMap<RoomModel, RoomResponseModel>();
                x.CreateMap<ServiceQuantityModel, ServiceQuantityResponseModel>();
                x.CreateMap<ServiceModel, ServiceResponseModel>();
                x.CreateMap<HotelModel, HotelResponseModel>()
                    .ForMember(x => x.Services , opt => opt.Ignore());
                x.CreateMap<LocationModel, LocationResponseModel>();
            });
            
            TokenConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<RefreshTokenRequestModel, TokenModel>();
            });

            ServiceConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<ServiceRequestModel, ServiceModel>();
                x.CreateMap<ServiceModel, ServiceResponseModel>();
            });
            
        }
    }
}