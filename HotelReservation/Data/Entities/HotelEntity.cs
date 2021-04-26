using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class HotelEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int LocationId { get; set; }
        public LocationEntity Location { get; set; }

        public List<RoomEntity> Rooms { get; set; }
        public int Rating { get; set; }
        
    }
}
