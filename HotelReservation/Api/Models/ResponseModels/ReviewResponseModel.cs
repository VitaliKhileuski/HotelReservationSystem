using System;
using System.Collections.Generic;

namespace HotelReservation.Api.Models.ResponseModels
{
    public class ReviewResponseModel
    {
        public UserResponseViewModel User { get; set; }
        public HotelResponseModel Hotel { get; set; }
        public OrderResponseModel Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Comment { get; set; }
        public ICollection<ReviewCategoryWithRatingResponseModel> Ratings { get; set; }
    }
}
