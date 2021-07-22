
using System;

namespace Business.Models.FilterModels
{
    public class HotelFilterModel
    {
        public string UserId { get; set; }
        public DateTime? CheckInDate { get; set; }
        public DateTime? CheckOutDate { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string HotelName { get; set; }
        public string Email { get; set; }
        public string Surname { get; set; }
    }
}
