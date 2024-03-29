﻿using System;
using System.Collections.Generic;

namespace Business.Models
{
   public class RoomModel
    {
        public Guid Id { get; set; }
        public  UserModel User { get; set; }
        public  ICollection<OrderModel> Orders { get; set; }
        public ICollection<AttachmentModel> Attachments { get; set; }
        public Guid HotelId { get; set; }
        public  HotelModel Hotel { get; set; }
        public string RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public decimal PaymentPerDay { get; set; }
    }
}