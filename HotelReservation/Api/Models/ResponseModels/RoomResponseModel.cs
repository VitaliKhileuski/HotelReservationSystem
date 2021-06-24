namespace HotelReservation.Api.Models.ResponseModels
{
    public class RoomResponseModel
    {
        public HotelResponseModel Hotel { get; set; }
        public string Id { get; set; }
        public string RoomNumber { get; set; }
        public int BedsNumber { get; set; }
        public double PaymentPerDay { get; set; }

    }
}