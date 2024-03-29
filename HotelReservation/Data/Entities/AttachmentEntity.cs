﻿using System;

namespace HotelReservation.Data.Entities
{
    public class AttachmentEntity : Entity
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public virtual RoomEntity Room { get; set;}
        public virtual HotelEntity Hotel { get; set; }
        public Guid FileContentId { get; set; }
        public virtual FileContentEntity FileContent { get; set; }
    }
}