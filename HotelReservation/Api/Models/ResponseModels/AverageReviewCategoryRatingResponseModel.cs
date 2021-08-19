using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Api.Models.ResponseModels
{
    public class AverageReviewCategoryRatingResponseModel
    {
        public string CategoryName { get; set; }
        public double? AverageRating { get; set; }
        public int NumberOfReviews { get; set; }
    }
}
