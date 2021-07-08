using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Data.Entities
{
    public class ServiceQuantityEntity : Entity
    {
        public Guid ServiceId { get; set; }
        public virtual ServiceEntity Service { get; set; }
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }
        public virtual OrderEntity Order { get; set; }
    }
}
