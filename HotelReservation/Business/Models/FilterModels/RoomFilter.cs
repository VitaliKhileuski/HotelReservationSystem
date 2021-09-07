using System;

namespace Business.Models.FilterModels
{
    public class RoomFilter
    {
        public string UserId { get; set; }
        public string RoomNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
