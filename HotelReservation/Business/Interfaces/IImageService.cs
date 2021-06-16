using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IImageService
    { 
        Task AddImageToHotel(string image, int hotelId, int userId);
        Task<string> GetHotelImage(int hotelId);
        Task<List<string>> GetRoomImages(int roomId);
        Task SetImagesToRoom(List<string> imagesData,int roomId,int userId);
    }
}
