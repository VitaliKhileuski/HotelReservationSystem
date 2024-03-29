﻿using System;
using System.Collections.Generic;

namespace Business.Models
{
    public class ServiceModel
    {
        public Guid Id { get; set; }
        public HotelModel Hotel { get; set; }
        public ICollection<RoomModel> Rooms { get; set; }
        public string Name { get; set; }
        public decimal Payment { get; set; }
    }
}