using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Api.Models.ResponseModels
{
    public class OrderResponseModel
    {
        public ICollection<ServiceResponseModel> Services;
        public RoomResponseModel Room { get; set; }
        public DateTime DateOrdered { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays { get; set; }
        public double FullPrice { get; set; }
    }
}
