using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Models
{
    public class UserModel
    {

        public  ICollection<RoomModel> Rooms { get; set; }
        public  ICollection<OrderModel> Orders { get; set; }
        public RoleModel Role { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Birthdate { get; set; }
    }
}
