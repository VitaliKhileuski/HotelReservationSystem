using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelReservation.Api.Models.RequestModels
{
    public class LoginUserRequestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
