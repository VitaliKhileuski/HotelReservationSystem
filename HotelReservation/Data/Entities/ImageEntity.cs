namespace HotelReservation.Data.Entities
{
    public class ImageEntity : Entity
    {
        public byte[] ImageData { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }

    }
}
