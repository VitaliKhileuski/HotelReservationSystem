using System;

namespace HotelReservation.Data.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
    }
}