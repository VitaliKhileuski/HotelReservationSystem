using System;

namespace HotelReservation.Data.Entities
{
    public class ReviewCategoryWithRatingEntity : Entity
    {
        public Guid CategoryId { get; set; }
        public virtual ReviewCategoryEntity Category { get; set; }
        public Guid ReviewId { get; set; }
        public virtual ReviewEntity Review { get; set; }
        public double Rating { get; set; }
    }
}
