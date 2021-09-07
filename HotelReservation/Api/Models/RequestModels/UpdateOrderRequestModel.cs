using System;

namespace HotelReservation.Api.Models.RequestModels
{
    public class UpdateOrderRequestModel
    {
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
