using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
    public class TokenConfiguration : MapperConfiguration
    {
        public TokenConfiguration() : base(x =>
        {
            x.CreateMap<RefreshTokenEntity, TokenModel>();
            x.CreateMap<TokenModel, RefreshTokenEntity>();
        })
        {

        }
    }
}
