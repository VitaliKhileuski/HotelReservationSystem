using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Api.Models.ResponseModels
{
    public class RoomResponseModel
    {
        public int Id { get; set; }
        public string RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public double PaymentPerDay { get; set; }
        public bool IsEmpty { get; set; }

    }
}
