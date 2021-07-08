using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Api.Models.ResponseModels
{
    public class ServiceQuantityResponseModel
    {
        public Guid Id { get; set; }
        public ServiceResponseModel Service { get; set; }
        public int Quantity { get; set; }
    }
}
