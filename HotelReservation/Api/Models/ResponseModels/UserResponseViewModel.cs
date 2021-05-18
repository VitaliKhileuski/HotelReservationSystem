using System;
using System.Collections.Generic;

namespace HotelReservation.Api.Models.ResponseModels
{
    public class UserResponseViewModel
    {
        public int Id { get; set; }
        public RoleResponseModel Role { get; set; }
        public ICollection<OrderResponseModel> Orders { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthdate { get; set; }
    }
}