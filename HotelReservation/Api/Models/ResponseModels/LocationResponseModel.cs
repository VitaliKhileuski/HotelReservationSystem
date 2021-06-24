namespace HotelReservation.Api.Models.ResponseModels
{
    public class LocationResponseModel
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
    }
}