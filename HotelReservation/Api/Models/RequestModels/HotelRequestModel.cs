using System.Collections.Generic;

namespace HotelReservation.Api.Models.RequestModels
{
    public class HotelRequestModel
    {
        public string Name { get; set; }
        public LocationRequestModel Location { get; set; }
        public ICollection<RoomRequestModel> Rooms { get; set; }
    }
}