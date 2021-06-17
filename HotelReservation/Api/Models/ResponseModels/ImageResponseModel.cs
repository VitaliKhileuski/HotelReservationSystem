using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Api.Models.ResponseModels
{
    public class ImageResponseModel
    {
        public string ImageBase64 { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
    }
}
