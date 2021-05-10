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
        public MapperConfiguration RoleConfiguration;
        public MapperConfiguration RoomConfiguration;
        public MapperConfiguration OrderConfiguration;

        public CustomMapperConfiguration()
        {
            HotelConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<HotelModel, HotelResponseModel>();
                x.CreateMap<HotelRequestModel, HotelModel>();
                x.CreateMap<LocationModel, LocationResponseModel>();
                x.CreateMap<RoomModel, RoomResponseModel>();
            });
            RoleConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<RoleModel, RoleResponseModel>();
                x.CreateMap<UserModel, UserResponseViewModel>()
                    .ForMember(x => x.Role, opt => opt.Ignore());
            });
            UsersConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<LoginUserRequestModel, LoginUserModel>();
                x.CreateMap<RegisterUserRequestModel, RegisterUserModel>();
                x.CreateMap<UserModel, UserResponseViewModel>();
                x.CreateMap<RoleModel, RoleResponseModel>();
            });
            RoomConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<RoomModel, RoomResponseModel>();
                x.CreateMap<RoomRequestModel, RoomModel>();
            });
            OrderConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<OrderRequestModel, OrderModel>();
            });
        }
    }
}
