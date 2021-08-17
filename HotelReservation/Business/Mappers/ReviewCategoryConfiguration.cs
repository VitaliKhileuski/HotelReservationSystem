using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
    public class ReviewCategoryConfiguration : MapperConfiguration
    {
        public ReviewCategoryConfiguration() : base(x =>
        {
            x.CreateMap<ReviewCategoryEntity, ReviewCategoryModel>().ReverseMap();
        })
        {

        }
    }
}
