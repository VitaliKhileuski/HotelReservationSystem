using System;
using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class UserEntity
    {
        public int RoleId { get; set; }
        public virtual RoleEntity Role { get; set; }
        public virtual ICollection<HotelEntity> OwnedHotels { get; set; }
        public virtual  ICollection<RoomEntity> Rooms { get; set; }
        public virtual ICollection<OrderEntity> Orders { get; set; }
        public int? RefreshTokenId { get; set; }
        public virtual RefreshTokenEntity RefreshToken { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }
}