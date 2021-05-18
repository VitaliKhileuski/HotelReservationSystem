using System.Collections.Generic;

namespace Business.Models
{
    public class HotelModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int HotelAdminId { get; set; }
        public LocationModel Location { get; set; }
        public ICollection<RoomModel> Rooms { get; set; }
        public ICollection<ServiceModel> Services { get; set; }
    }
}