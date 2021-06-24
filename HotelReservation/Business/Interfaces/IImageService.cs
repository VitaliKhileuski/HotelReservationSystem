using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IImageService
    {
        Task AddImageToHotel(ImageModel image, Guid hotelId,string userId);
        Task<ImageModel> GetHotelImage(Guid hotelId);
        Task<List<ImageModel>> GetRoomImages(Guid roomId);
        Task SetImagesToRoom(List<ImageModel> imagesData,Guid roomId,string userId);
    }
}