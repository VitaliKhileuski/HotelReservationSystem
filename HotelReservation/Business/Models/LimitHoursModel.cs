using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class LimitHoursModel
    {
        public TimeSpan CheckInTime { get; set; }
        public TimeSpan CheckOutTime { get; set; }
    }
}
