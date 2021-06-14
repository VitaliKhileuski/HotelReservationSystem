using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IImageService
    { 
        Task AddImageToHotel(string image, int hotelId, int userId);
        Task<string> GetHotelImage(int hotelId, int userId);
    }
}
