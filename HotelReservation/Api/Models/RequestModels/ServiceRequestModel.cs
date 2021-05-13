using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Api.Models.RequestModels
{
    public class ServiceRequestModel
    {
        public string Name { get; set; }
        public double Payment { get; set; }
    }
}
