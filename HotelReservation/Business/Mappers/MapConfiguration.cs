using System;
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
        public MapperConfiguration AttachmentConfiguration;
        public MapperConfiguration ServiceQuantityConfiguration;
        public MapperConfiguration ReviewCategoryConfiguration;
        public MapperConfiguration ReviewCategoryWithRatingConfiguration;
        public MapperConfiguration ReviewConfiguration;

        public MapConfiguration()
        {
            UserConfiguration = new UserConfiguration();
            HotelConfiguration = new HotelConfiguration();
            LocationConfiguration = new LocationConfiguration();
            OrderConfiguration = new OrderConfiguration();
            RoomConfiguration = new RoomConfiguration();
            RoleConfiguration = new RoleConfiguration();
            TokenConfiguration = new TokenConfiguration();
            ServiceConfiguration = new ServiceConfiguration();
            AttachmentConfiguration = new AttachmentConfiguration();
            ServiceQuantityConfiguration = new ServiceQuantityConfiguration();
            ReviewCategoryConfiguration = new ReviewCategoryConfiguration();
            ReviewCategoryWithRatingConfiguration = new ReviewCategoryConfiguration();
            ReviewConfiguration = new ReviewConfiguration();
        }
    }
}