
using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
   public  class RoomEntity : Entity
    {

        public int? UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public virtual ICollection<OrderEntity> Order { get; set; }
        public int HotelId { get; set; }
        public virtual HotelEntity Hotel { get; set; }
        public string RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public decimal PaymentPerDay { get; set; }
    }
}