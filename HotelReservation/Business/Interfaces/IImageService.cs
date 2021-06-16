using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IImageService
    { 
        Task AddImageToHotel(string image, int hotelId, int userId);
        Task<string> GetHotelImage(int hotelId, int userId);
        Task<List<string>> GetRoomImages(int roomId,int userId);
        Task SetImagesToRoom(List<string> imagesData,int roomId,int userId);
    }
}
