using System;
using System.Collections.Generic;

namespace HotelReservation.Api.Models.RequestModels
{
    public class HotelRequestModel
    {
        public string Name { get; set; }
        public int? LimitDays { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public LocationRequestModel Location { get; set; }
        public ICollection<RoomRequestModel> Rooms { get; set; }
    }
}