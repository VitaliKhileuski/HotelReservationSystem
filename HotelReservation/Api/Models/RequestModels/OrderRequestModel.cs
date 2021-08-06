using System;
using System.Collections.Generic;

namespace HotelReservation.Api.Models.RequestModels
{
    public class OrderRequestModel
    {
        public string UserEmail { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public ICollection<ServiceQuantityRequestModel> ServiceQuantities { get; set; }
    }
}