namespace HotelReservation.Data.Entities
{
   public  class RoomEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int OrderId { get; set; }
        public OrderEntity Order { get; set; }

        public int HotelId { get; set; }
        public HotelEntity Hotel { get; set; }

        public int RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public double PaymentPerDay { get; set; }
        public bool WiFi { get; set; }
        public bool MiniBar { get; set; }
        public bool IsEmpty { get; set; }


    }
}
