using System;
using System.Collections.Generic;

namespace HotelReservation.Api.Models.RequestModels
{
    public class OrderRequestModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<ServiceQuantityRequestModel> ServiceQuantities { get; set; }
    }
}