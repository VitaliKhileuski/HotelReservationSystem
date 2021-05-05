using System.Collections.Generic;

namespace Business.Models
{
    public class HotelModel
    {
        public string Name { get; set; }
        public LocationModel Location { get; set; }
        public ICollection<RoomResponseModel> Rooms { get; set; }
        public int Rating { get; set; }

    }
}
