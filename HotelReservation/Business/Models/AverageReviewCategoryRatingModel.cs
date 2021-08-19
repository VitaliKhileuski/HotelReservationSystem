using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class AverageReviewCategoryRatingModel
    {
        public string CategoryName { get; set; }
        public double? AverageRating { get; set; }
        public int NumberOfReviews { get; set; }
    }
}
