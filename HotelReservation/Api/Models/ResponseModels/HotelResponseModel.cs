using System.Collections.Generic;

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