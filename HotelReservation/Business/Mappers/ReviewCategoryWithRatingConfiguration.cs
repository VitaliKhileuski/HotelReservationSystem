using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
    public class ReviewCategoryWithRatingConfiguration : MapperConfiguration
    {
        public ReviewCategoryWithRatingConfiguration() : base(x =>
        {
            x.CreateMap<ReviewCategoryWithRatingEntity, ReviewCategoryWithRatingModel>().ReverseMap();
            x.CreateMap<ReviewCategoryEntity, ReviewCategoryModel>().ReverseMap();
        })
        {

        }
    }
}
