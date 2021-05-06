using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Api.Models.RequestModels;

namespace HotelReservation.Api.Models.ResponseModels
{
    public class HotelResponseModel
    {
        public string Name { get; set; }
        public LocationResponseModel Location { get; set; }
        public ICollection<RoomRequestModel> Rooms { get; set; }
        public int Rating { get; set; }
    }
}
