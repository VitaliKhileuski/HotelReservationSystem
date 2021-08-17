using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Data.Entities
{
    public class ReviewEntity : Entity
    {
        public Guid UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public Guid OrderId { get; set; }
        public virtual OrderEntity Order { get; set; }
        public  DateTime CreatedAt { get; set; }
        public string Comment { get; set;}
        public virtual ICollection<ReviewCategoryWithRatingEntity> Ratings { get; set; }
        public bool Advised { get; set; }


    }
}
