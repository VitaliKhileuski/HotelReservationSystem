﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Api.Models.RequestModels
{
    public class ReviewCategoryRequestModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
