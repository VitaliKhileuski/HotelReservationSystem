﻿using System;

namespace HotelReservation.Data.Entities
{
    public class LocationEntity : Entity
    {
        public Guid HotelId { get; set; }
        public virtual HotelEntity Hotel { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
    }
}