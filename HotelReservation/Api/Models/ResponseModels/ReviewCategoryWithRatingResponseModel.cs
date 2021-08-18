using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Api.Models.ResponseModels
{
    public class ReviewCategoryWithRatingResponseModel
    {
        public ReviewCategoryResponseModel Category { get; set; }
        public double Rating { get; set; }
    }
}
