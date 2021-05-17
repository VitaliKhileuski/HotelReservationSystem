
namespace Business.Models
{
   public class RoomModel
    {
        public int Id { get; set; }
        public  UserModel User { get; set; }
        public  OrderModel Order { get; set; }
        public  HotelModel Hotel { get; set; }
        public string RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public decimal PaymentPerDay { get; set; }
       
        public bool IsEmpty { get; set; }
    }
}