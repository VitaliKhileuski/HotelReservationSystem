using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class ReviewModel
    {
        public UserModel User { get; set; }
        public  HotelModel Hotel { get; set; }
        public Guid OrderId { get; set; }
        public OrderModel Order { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Comment { get; set; }
        public virtual ICollection<ReviewCategoryWithRatingModel> Ratings { get; set; }
        public double? AverageRating { get; set; }

    }
}
