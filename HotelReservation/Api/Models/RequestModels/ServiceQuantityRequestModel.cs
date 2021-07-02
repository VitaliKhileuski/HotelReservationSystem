using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Api.Models.RequestModels
{
    public class ServiceQuantityRequestModel
    {
        public Guid ServiceId { get; set; }
        public int Quantity { get; set; }
    }
}
