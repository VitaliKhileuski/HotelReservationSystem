using System;
using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class UserEntity : Entity
    {
        public Guid RoleId { get; set; }
        public virtual RoleEntity Role { get; set; }
        public virtual ICollection<HotelEntity> OwnedHotels { get; set; }
        public virtual ICollection<OrderEntity> Orders { get; set; }
        public Guid? RefreshTokenId { get; set; }
        public virtual RefreshTokenEntity RefreshToken { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public bool IsVerified { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var user = (UserEntity)obj;
            return user.Id == Id;
        }


        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}