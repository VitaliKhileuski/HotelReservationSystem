using System;
using System.Collections.Generic;
using System.Text;
using Business.Models.ResponseModels;

namespace Business.Models.RequestModels
{
    public class HotelRequestModel
    {
        public string Name { get; set; }
        public LocationModel Location { get; set; }
        public ICollection<RoomResponseModel> Rooms { get; set; }
        public int Rating { get; set; }

    }
}
