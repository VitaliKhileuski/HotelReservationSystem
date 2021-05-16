using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class ServiceModel
    {
        public int Id { get; set; }
        public HotelModel Hotel { get; set; }
        public ICollection<RoomModel> Rooms { get; set; }
        public string Name { get; set; }
        public double Payment { get; set; }
    }
}
