using System;
using System.Collections.Generic;
using System.Text;

namespace HotelReservation.Data.Entities
{
   public class RefreshTokenEntity
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public  virtual UserEntity User { get; set; }
    }
}