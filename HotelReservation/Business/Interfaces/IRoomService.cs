using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;
using Business.Models.FilterModels;

namespace Business.Interfaces
{
    public interface IRoomService
    {
        Task AddRoom(Guid hotelId, RoomModel room, string userId); 
       // Task<ICollection<RoomModel>> GetRoomsFromHotel(Guid hotelId,DateTime checkInDate, DateTime checkOutDate);
        Task UpdateRoom(Guid roomId, string userId, RoomModel room);
        Task DeleteRoom(Guid roomId, string userId);
        Task<PageInfo<RoomModel>> GetRoomsPage(Guid hotelId,RoomFilter roomFilter, Pagination roomPagination, SortModel sortModel);
        Task<bool> IsRoomEmpty(Guid roomId, DateTime checkInDate, DateTime checkOutDate);
        Task<bool> IsRoomBlocked(Guid roomId);
        Task BlockRoomById(Guid roomId,string userId);
    }
}