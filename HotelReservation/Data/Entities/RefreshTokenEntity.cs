
namespace HotelReservation.Data.Entities
{
   public class RefreshTokenEntity : Entity
    {
        public string Token { get; set; }
        public  virtual UserEntity User { get; set; }
    }
}