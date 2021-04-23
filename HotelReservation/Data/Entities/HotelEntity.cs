using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class HotelEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int CountryId { get; set; }
        public CountryEntity Country { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public List<RoomEntity> Rooms { get; set; }
        public int Rating { get; set; }
        
    }
}
