﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models.ResponseModels
{
    public class LocationResponseModel
    {
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
    }
}