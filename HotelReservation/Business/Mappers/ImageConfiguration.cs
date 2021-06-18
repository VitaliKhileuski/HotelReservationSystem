using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
    public class ImageConfiguration : MapperConfiguration
    {
        public ImageConfiguration() : base(x =>
        {
            x.CreateMap<ImageEntity, ImageModel>()
                .ForMember(imageModel => imageModel.ImageBase64, opt => opt.MapFrom(image => ImageConverter(image.ImageData)));
            x.CreateMap<ImageModel, ImageEntity>()
                .ForMember(image => image.ImageData, opt => opt.MapFrom(image => ImageConverter(image.ImageBase64)));
        })
        {

        }
        private static byte[] ImageConverter(string image)
        {
            var imageData = Convert.FromBase64String(image);

            return imageData;
        }

        private static string ImageConverter(byte[] image)
        {
            var base64Image = Convert.ToBase64String(image);
            return base64Image;
        }
    }
}
