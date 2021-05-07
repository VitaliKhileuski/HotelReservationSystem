using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Api.Models.RequestModels
{
    public class HotelRequestModel
    {
        public string Name { get; set; }
        public LocationRequestModel Location { get; set; }
        public ICollection<RoomRequestModel> Rooms { get; set; }
    }
}
