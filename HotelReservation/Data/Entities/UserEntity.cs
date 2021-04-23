using System;

namespace HotelReservation.Data.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public RoleEntity Role { get; set; }
        public DateTime Birthdate { get; set; }

    }
}
