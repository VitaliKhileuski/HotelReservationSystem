using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Data.Entities
{
    public class AverageReviewCategoryRatingsEntity : Entity
    {
        public Guid CategoryId { get; set; }
        public virtual ReviewCategoryEntity Category { get; set; }
        public Guid HotelId { get; set; }
        public virtual  HotelEntity Hotel { get; set; }
        public double? AverageRating { get; set; }
        public int NumberOfReviews { get; set; }
    }
}
