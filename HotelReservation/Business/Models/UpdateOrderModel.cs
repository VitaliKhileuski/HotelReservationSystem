using System;

namespace Business.Models
{
    public class UpdateOrderModel
    {
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
    }
}
