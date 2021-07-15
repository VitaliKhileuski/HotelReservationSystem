using System;
using System.Collections.Generic;

namespace HotelReservation.Data.Entities
{
   public  class RoomEntity : Entity
    {
        public virtual ICollection<OrderEntity> Orders { get; set; }
        public virtual List<AttachmentEntity> Attachments { get; set; }
        public Guid HotelId { get; set; }
        public virtual HotelEntity Hotel { get; set; }
        public string RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public decimal PaymentPerDay { get; set; }
        public DateTime? UnblockDate { get; set; }
        public string PotentialCustomerId { get; set; }
    }
}
