using System;
using System.Collections.Generic;

namespace HotelReservation.Api.Models.ResponseModels
{
    public class RoomResponseModel
    {
        public Guid HotelId { get; set; }
        public HotelResponseModel Hotel { get; set; }
        public string Id { get; set; }
        public string RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public double PaymentPerDay { get; set; }
        public ICollection<string> ImageUrls { get; set; }

    }
}