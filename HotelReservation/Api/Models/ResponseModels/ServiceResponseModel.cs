using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Api.Models.ResponseModels
{
    public class ServiceResponseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Payment { get; set; }
    }
}
