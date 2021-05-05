using System.Collections.Generic;

namespace Business.Models
{
    public class HotelResponseModel
    {
        public string Name { get; set; }
        public LocationModel Location { get; set; }
        public  List<RoomResponseModel> Rooms { get; set; }
        public int Rating { get; set; }
    }
}