using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models.ResponseModels
{
   public class RoomResponseModel
    {
        public int RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public double PaymentPerDay { get; set; }
        public bool WiFi { get; set; }
        public bool MiniBar { get; set; }
        public bool IsEmpty { get; set; }
    }
}