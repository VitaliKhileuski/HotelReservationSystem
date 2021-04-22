using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLayer.Entities
{
   public  class Room
    {
        public int RoomNumber { get; set; }
        public int BedsNumber { get; set; }

        public double PaymentPerDay { get; set; }

        public bool WiFi { get; set; }
        public bool MiniBar { get; set; }


    }
}
