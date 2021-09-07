using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Mappers
{
    class AttachmentConfiguration : MapperConfiguration
    {
        public AttachmentConfiguration() : base(x =>
        {
            x.CreateMap<AttachmentModel, AttachmentEntity>().ReverseMap();
            x.CreateMap<FileContentModel, FileContentEntity>().ReverseMap();
        })
        {

        }
    }
}
