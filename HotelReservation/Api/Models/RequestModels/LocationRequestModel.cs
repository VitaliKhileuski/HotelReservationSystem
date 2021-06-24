namespace HotelReservation.Api.Models.RequestModels
{
    public class LocationRequestModel
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
    }
}