using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Models;
using HotelReservation.Api.Models.RequestModels;

namespace HotelReservation.Api.Mappers
{
    public class AttachmentConfiguration : MapperConfiguration
    {
        public AttachmentConfiguration() : base(x =>
        {
            x.CreateMap<FileRequestModel, AttachmentModel>()
                .ForPath(file => file.FileContent.Content, opt => opt.MapFrom(image => ImageConverter(image.FileBase64)));
        })
        {

        }
        private static byte[] ImageConverter(string image)
        {
            var imageData = Convert.FromBase64String(image);

            return imageData;
        }
    }
}
