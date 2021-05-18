using System;
using System.Collections.Generic;

namespace HotelReservation.Api.Models.RequestModels
{
    public class OrderRequestModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<int> ServicesId { get; set; }
    }
}