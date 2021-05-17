using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
   public class ServiceEntity
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public virtual HotelEntity Hotel { get; set; }
        public virtual ICollection<OrderEntity> Orders { get; set; }
        public string Name { get; set; }
        public decimal Payment { get; set; }
    }
}
