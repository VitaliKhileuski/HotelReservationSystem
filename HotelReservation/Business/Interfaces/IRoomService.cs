using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IRoomService
    {
        Task AddRoom(int hotelId, RoomModel room, int userId); 
        Task<ICollection<RoomModel>> GetRoomsFromHotel(int hotelId);
        Task UpdateRoom(int roomId, int userId, RoomModel room);
        Task DeleteRoom(int roomId, int userId);
        Task<Tuple<IEnumerable<RoomModel>, int>> GetRoomsPage(int hotelId, Pagination hotelPagination);

    }
}