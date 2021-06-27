namespace HotelReservation.Api.Models.RequestModels
{
    public class FileRequestModel
    {
        public string FileBase64 { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
    }
}
