using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IRoomService
    {
        public Task AddRoom(int hotelId, RoomModel room, int userId);
        public Task<ICollection<RoomModel>> GetAllRooms();
        public Task<ICollection<RoomModel>> GetRoomsFromHotel(int hotelId);
        public Task UpdateRoom(int roomId, int userId, RoomModel room);
        public Task DeleteRoom(int roomId, int userId);

    }
}