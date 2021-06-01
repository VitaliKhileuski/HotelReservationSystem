using System;
using System.Collections.Generic;

namespace Business.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public  ICollection<RoomModel> Rooms { get; set; }
        public  ICollection<OrderModel> Orders { get; set; }
        public ICollection<HotelModel> OwnedHotels { get; set; }
        public int RoleId { get; set; }
        public RoleModel Role { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
    }
}