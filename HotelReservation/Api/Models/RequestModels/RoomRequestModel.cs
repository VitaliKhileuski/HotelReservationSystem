namespace HotelReservation.Api.Models.RequestModels
{
    public class RoomRequestModel
    {
        public string RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public decimal PaymentPerDay { get; set; }
    }
}