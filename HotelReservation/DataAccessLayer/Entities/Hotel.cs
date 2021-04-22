using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Entities
{
    public class Hotel
    {
        public string Name { get; set; }
        public Country Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public bool Parking { get; set; }
        public List<Room> Rooms { get; set; }
        public int Rating { get; set; }
        
    }
}
