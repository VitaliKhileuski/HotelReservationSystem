using System;
using System.Collections;
using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class UserEntity
    {
        public int RoleId { get; set; }
        public virtual RoleEntity Role { get; set; }

        public virtual  ICollection<RoomEntity> Rooms { get; set; }
        public virtual ICollection<OrderEntity> Orders { get; set; }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public DateTime Birthdate { get; set; }

    }
}
