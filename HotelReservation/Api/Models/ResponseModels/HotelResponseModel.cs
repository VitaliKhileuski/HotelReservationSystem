using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Api.Models.RequestModels;

namespace HotelReservation.Api.Models.ResponseModels
{
    public class HotelResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HotelAdminId { get; set; }
        public LocationResponseModel Location { get; set; }
        public ICollection<RoomResponseModel> Rooms { get; set; }
        public ICollection<ServiceResponseModel> Services { get; set; }

    }
}
