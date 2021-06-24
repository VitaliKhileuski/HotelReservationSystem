using System;
using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
    public class HotelEntity : Entity
    {
        public string Name { get; set; }
        public virtual LocationEntity Location { get; set; }
        public virtual List<RoomEntity> Rooms { get; set; }
        public virtual List<ServiceEntity> Services { get; set; }
        public virtual ICollection<UserEntity> Admins { get; set; }
        public Guid? ImageId { get; set; }
        public virtual ImageEntity Image { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var hotel = (HotelEntity)obj;
            return hotel.Id == Id;
        }


        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

    }
}