using System;
using System.Collections.Generic;

namespace HotelReservation.Api.Models.ResponseModels
{
    public class OrderResponseModel
    {
        public string Id { get; set; }
        public ICollection<ServiceQuantityResponseModel> Services { get; set; }
        public RoomResponseModel Room { get; set; }
        public DateTime DateOrdered { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays { get; set; }
        public decimal FullPrice { get; set; }
    }
}