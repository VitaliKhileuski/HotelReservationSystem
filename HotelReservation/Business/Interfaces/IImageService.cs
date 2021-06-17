using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IImageService
    {
        Task AddImageToHotel(ImageModel image, int hotelId, int userId);
        Task<ImageModel> GetHotelImage(int hotelId);
        Task<List<ImageModel>> GetRoomImages(int roomId);
        Task SetImagesToRoom(List<ImageModel> imagesData,int roomId,int userId);
    }
}
