using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Data.Entities
{
    public class EmailVerificationEntity : Entity
    {
        public Guid UserId { get; set; }
        public string VerificationCode { get; set; }
        public DateTime ExpiresOn { get; set; }

    }
}
