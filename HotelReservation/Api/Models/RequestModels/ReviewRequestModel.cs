using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Api.Models.RequestModels
{
    public class ReviewRequestModel
    {
        public ICollection<ReviewCategoryWithRatingRequestModel> Ratings;
        public string Comment { get; set; }
    }
}
