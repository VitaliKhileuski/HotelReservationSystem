using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class RoleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UserEntity> Users { get; set; }
    }
}
