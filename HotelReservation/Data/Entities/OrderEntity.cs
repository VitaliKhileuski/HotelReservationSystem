using System;

namespace HotelReservation.Data.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public UserEntity Customer { get; set; }
        public int RoomId { get; set; }
        public RoomEntity Room { get; set; }

        public DateTime DateOrdered { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays { get; set; }
        public double FullPrice { get; set; }

    }
}
