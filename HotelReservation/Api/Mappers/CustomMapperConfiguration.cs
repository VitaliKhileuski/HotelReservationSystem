using AutoMapper;
using Business.Models;
using HotelReservation.Api.Models.RequestModels;
using HotelReservation.Api.Models.ResponseModels;

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
        public AttachmentConfiguration AttachmentConfiguration;

        public CustomMapperConfiguration()
        {
            AttachmentConfiguration = new AttachmentConfiguration();
            HotelConfiguration = new  HotelConfiguration();
            
            UsersConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<LoginUserRequestModel, LoginUserModel>();
                x.CreateMap<RegisterUserRequestModel, RegisterUserModel>();
                x.CreateMap<UserRequestModel, UserModel>();
                x.CreateMap<UserModel, UserResponseViewModel>();
                x.CreateMap<RoleModel, RoleResponseModel>();
                x.CreateMap<OrderModel, OrderResponseModel>();
                x.CreateMap<RoomModel, RoomResponseModel>();
                x.CreateMap<ServiceModel, ServiceResponseModel>();
                x.CreateMap<HotelModel, HotelResponseModel>()
                    .ForMember(x => x.Rooms, opt => opt.Ignore())
                    .ForMember(x => x.Services , opt => opt.Ignore());
                x.CreateMap<LocationModel, LocationResponseModel>();
            });
            RoomConfiguration = new RoomConfiguration();
            OrderConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<OrderRequestModel, OrderModel>();
                x.CreateMap<ServiceRequestModel, ServiceModel>()
                .ForMember(x => x.Hotel , opt => opt.Ignore());
                x.CreateMap<ServiceModel, ServiceResponseModel>();
                x.CreateMap<OrderModel, OrderResponseModel>();
                x.CreateMap<RoleModel, RoleResponseModel>();
                x.CreateMap<RoomModel, RoomResponseModel>();
                x.CreateMap<HotelModel, HotelResponseModel>()
                    .ForMember(X => X.Services, opt => opt.Ignore())
                    .ForMember(x => x.Rooms, opt => opt.Ignore());
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