﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Data.Entities
{
    public class LocationEntity
    {
        public int HotelId { get; set; }
        public HotelEntity Hotel { get; set; }
        
        public int Id { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }

    }
}
