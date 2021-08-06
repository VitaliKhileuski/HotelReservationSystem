using System;
using System.Collections.Generic;
using Org.BouncyCastle.Asn1.Cms;

namespace HotelReservation.Api.Models.ResponseModels
{
    public class OrderResponseModel
    {
        public string Id { get; set; }
        public ICollection<ServiceQuantityResponseModel> Services { get; set; }
        public UserResponseViewModel Customer { get; set; }
        public RoomResponseModel Room { get; set; }
        public DateTime DateOrdered { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Number { get; set; }
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
        public int NumberOfDays { get; set; }
        public decimal FullPrice { get; set; }
    }
}