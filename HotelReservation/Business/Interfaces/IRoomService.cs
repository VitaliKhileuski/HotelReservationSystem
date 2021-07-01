using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IRoomService
    {
        Task AddRoom(Guid hotelId, RoomModel room, string userId); 
       // Task<ICollection<RoomModel>> GetRoomsFromHotel(Guid hotelId,DateTime checkInDate, DateTime checkOutDate);
        Task UpdateRoom(Guid roomId, string userId, RoomModel room);
        Task DeleteRoom(Guid roomId, string userId);
        Task<PageInfo<RoomModel>> GetRoomsPage(Guid hotelId,DateTime checkInDate,DateTime checkOutDate, Pagination roomPagination);
    }
}