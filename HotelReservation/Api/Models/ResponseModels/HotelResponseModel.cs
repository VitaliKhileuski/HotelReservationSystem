using System;
using System.Collections.Generic;

namespace HotelReservation.Api.Models.ResponseModels
{
    public class HotelResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int? LimitDays { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public UserResponseViewModel Admin { get; set; }
        public LocationResponseModel Location { get; set; }
        public ICollection<ServiceResponseModel> Services { get; set; }
        public ICollection<string> ImageUrls { get; set; }
        public double? AverageRating { get; set; }
        public ICollection<AverageReviewCategoryRatingResponseModel> AverageCategoryRatings { get; set; }
    }
}