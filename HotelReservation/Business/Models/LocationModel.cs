﻿using System;

namespace Business.Models
{ 
    public class LocationModel
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public HotelModel Hotel { get; set; }
    }
}