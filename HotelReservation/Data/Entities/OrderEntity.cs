using System;
using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class OrderEntity : Entity
    {
        public Guid UserId { get; set; }
        public virtual UserEntity Customer { get; set; }
        public Guid RoomId { get; set; }
        public virtual RoomEntity Room { get; set; }
        public virtual ICollection<ServiceQuantityEntity> Services { get; set; }
        public DateTime DateOrdered { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays { get; set; }
        public decimal FullPrice { get; set; }
    }
}