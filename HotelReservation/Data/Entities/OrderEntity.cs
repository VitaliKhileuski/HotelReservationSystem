using System;
using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class OrderEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public virtual UserEntity Customer { get; set; }
        public int RoomId { get; set; }
        public virtual RoomEntity Room { get; set; }
        public virtual ICollection<ServiceEntity> Services { get; set; }

        public DateTime DateOrdered { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays { get; set; }
        public double FullPrice { get; set; }

    }
}