﻿
namespace HotelReservation.Data.Entities
{
   public  class RoomEntity
    {
        public int Id { get; set; }

        public int? UserId { get; set; }
        public virtual UserEntity User { get; set; }
        public virtual OrderEntity Order { get; set; }
        public int HotelId { get; set; }
        public virtual HotelEntity Hotel { get; set; }
        public string RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public decimal PaymentPerDay { get; set; }
        public bool? IsEmpty { get; set; }
    }
}