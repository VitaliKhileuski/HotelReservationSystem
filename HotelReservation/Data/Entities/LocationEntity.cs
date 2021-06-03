
namespace HotelReservation.Data.Entities
{
    public class LocationEntity : Entity
    {
        public int HotelId { get; set; }
        public virtual HotelEntity Hotel { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
    }
}