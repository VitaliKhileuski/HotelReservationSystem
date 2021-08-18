using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Api.Models.RequestModels
{
    public class ReviewCategoryWithRatingRequestModel
    {
        public ReviewCategoryRequestModel Category { get; set; }
        public double Rating { get; set; }
    }
}
