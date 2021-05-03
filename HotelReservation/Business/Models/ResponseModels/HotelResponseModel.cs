using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models.ResponseModels
{
    public class HotelResponseModel
    {
        public string Name { get; set; }
        public LocationResponseModel Location { get; set; }
        public  List<RoomResponseModel> Rooms { get; set; }
        public int Rating { get; set; }
    }
}