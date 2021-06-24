using System;
using System.Collections.Generic;

namespace Business.Models
{
    public class HotelModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public UserModel Admin { get; set; }
        public LocationModel Location { get; set; }
        public ICollection<RoomModel> Rooms { get; set; }
        public ICollection<ServiceModel> Services { get; set; }
    }
}