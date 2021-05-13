using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class HotelEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HotelAdminId { get; set; }
        public virtual LocationEntity Location { get; set; }

        public virtual List<RoomEntity> Rooms { get; set; }
        public virtual List<ServiceEntity> Services { get; set; }

    }
}