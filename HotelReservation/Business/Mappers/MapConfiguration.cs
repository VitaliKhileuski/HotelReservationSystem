using System;
using System.Collections.Generic;
using System.Text;
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

        public MapConfiguration()
        {
            UserConfiguration = new MapperConfiguration(x =>
            {
                 x.CreateMap<UserEntity, UserModel>();
                 x.CreateMap<UserModel, UserEntity>();
                 x.CreateMap<RoleEntity, RoleModel>();
                 x.CreateMap<RefreshTokenEntity, TokenModel>()
                     .ForMember(x => x.User, opt => opt.Ignore());
            });
            HotelConfiguration = new MapperConfiguration( x =>
            {
                
                x.CreateMap<HotelModel, HotelEntity>()
                    .ForMember(x => x.Location, opt => opt.Ignore());
                x.CreateMap<HotelEntity, HotelModel>();
                x.CreateMap<LocationEntity, LocationModel>()
                    .ForMember(x => x.Hotel, opt => opt.Ignore());
                x.CreateMap<RoomEntity, RoomModel>()
                    .ForMember(x => x.Hotel, opt => opt.Ignore());
            });
            LocationConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<LocationModel, LocationEntity>();
                x.CreateMap<LocationEntity, LocationModel>();
                x.CreateMap<HotelEntity, HotelModel>().ForMember(X => X.Location, opt => opt.Ignore());
            });
            OrderConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<OrderEntity, OrderModel>();
                x.CreateMap<OrderModel, OrderEntity>();
                x.CreateMap<UserEntity, UserModel>()
                    .ForMember(x => x.Orders, opt => opt.Ignore());
                x.CreateMap<RoomEntity, RoomModel>();
            });
            RoomConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<RoomEntity,RoomModel>();
                x.CreateMap<RoomModel, RoomEntity>();
                x.CreateMap<UserEntity, UserModel>()
                    .ForMember(x => x.Rooms, opt => opt.Ignore());
                x.CreateMap<HotelEntity,HotelModel>()
                    .ForMember(x => x.Rooms, opt => opt.Ignore());
                x.CreateMap<LocationEntity, LocationModel>();
            });
            RoleConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<RoleEntity, RoleModel>();
                x.CreateMap<RoleModel, RoleEntity>();
                x.CreateMap<UserEntity, UserModel>()
                    .ForMember(x => x.Role, opt => opt.Ignore());
            });
            TokenConfiguration = new MapperConfiguration(x =>
            {
                x.CreateMap<RefreshTokenEntity, TokenModel>();
                x.CreateMap<TokenModel, RefreshTokenEntity>();
            });
        }
    }
}

