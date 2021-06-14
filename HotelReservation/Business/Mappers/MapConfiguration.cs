using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
    public class MapConfiguration
    {
        public MapperConfiguration UserConfiguration;
        public MapperConfiguration HotelConfiguration;
        public MapperConfiguration LocationConfiguration;
        public MapperConfiguration OrderConfiguration;
        public MapperConfiguration RoomConfiguration;
        public MapperConfiguration RoleConfiguration;
        public MapperConfiguration TokenConfiguration;
        public MapperConfiguration ServiceConfiguration;
        public MapperConfiguration ImageConfiguration;

        public MapConfiguration()
        {
            UserConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<UserEntity, UserModel>().ReverseMap();
                x.CreateMap<RegisterUserModel, UserEntity>().ReverseMap();
                x.CreateMap<RoleEntity, RoleModel>();
                x.CreateMap<OrderEntity, OrderModel>();
                x.CreateMap<RoomEntity, RoomModel>()
                    .ForMember(x => x.Orders, opt => opt.Ignore());
                x.CreateMap<ServiceEntity, ServiceModel>()
                    .ForMember(x => x.Rooms, opt => opt.Ignore());
                x.CreateMap<HotelEntity, HotelModel>();
                x.CreateMap<LocationEntity, LocationModel>();
            });
            HotelConfiguration = new MapperConfiguration( x =>
            {

                x.CreateMap<HotelModel, HotelEntity>();
                x.CreateMap<HotelEntity, HotelModel>();
                x.CreateMap<LocationModel, LocationEntity>();
                x.CreateMap<LocationEntity, LocationModel>()
                    .ForMember(x => x.Hotel, opt => opt.Ignore());
                x.CreateMap<RoomEntity, RoomModel>()
                    .ForMember(x => x.Hotel, opt => opt.Ignore())
                    .ForMember(x => x.Orders,opt => opt.Ignore())
                    .ForMember(x => x.User, opt => opt.Ignore());
                x.CreateMap<ServiceEntity, ServiceModel>();
                x.CreateMap<UserEntity, UserModel>()
                    .ForMember(x => x.OwnedHotels, opt => opt.Ignore())
                    .ForMember(x => x.Orders, opt => opt.Ignore())
                    .ForMember(x => x.Rooms, opt => opt.Ignore())
                    .ForMember(x => x.Role, opt => opt.Ignore());
            });
            LocationConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<LocationModel, LocationEntity>().ReverseMap();
                x.CreateMap<HotelEntity, HotelModel>().ForMember(x => x.Location, opt => opt.Ignore());
            });
            OrderConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<OrderEntity, OrderModel>().ReverseMap();
                x.CreateMap<UserEntity, UserModel>()
                    .ForMember(x => x.Orders, opt => opt.Ignore());
                x.CreateMap<RoomEntity, RoomModel>();
                x.CreateMap<ServiceModel, ServiceEntity>().ReverseMap();
                x.CreateMap<RoleEntity, RoleModel>();
                x.CreateMap<HotelEntity, HotelModel>();
                x.CreateMap<LocationEntity, LocationModel>();
            });
            RoomConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<RoomEntity, RoomModel>().ReverseMap();
                x.CreateMap<UserEntity, UserModel>()
                    .ForMember(x => x.Rooms, opt => opt.Ignore())
                    .ForMember(x => x.Role, opt => opt.Ignore());
                x.CreateMap<HotelEntity,HotelModel>()
                    .ForMember(x => x.Rooms, opt => opt.Ignore());
                x.CreateMap<LocationEntity, LocationModel>();
                x.CreateMap<ServiceEntity,ServiceModel>();
                x.CreateMap<OrderEntity, OrderModel>()
                .ForMember(x => x.Room, opt => opt.Ignore())
                .ForMember(x => x.Customer, opt =>opt.Ignore());
            });
            RoleConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<RoleEntity, RoleModel>().ReverseMap();
                x.CreateMap<UserEntity, UserModel>()
                    .ForMember(x => x.Role, opt => opt.Ignore());
            });
            TokenConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<RefreshTokenEntity, TokenModel>();
                x.CreateMap<TokenModel, RefreshTokenEntity>();
            });
            ServiceConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<ServiceEntity, ServiceModel>()
                    .ForMember(x => x.Hotel, opt => opt.Ignore())
                    .ForMember(x => x.Rooms, opt => opt.Ignore());
                x.CreateMap<ServiceModel, ServiceEntity>();


            });
            ImageConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<ImageEntity, ImageModel>().ReverseMap();
            });
        }
    }
}