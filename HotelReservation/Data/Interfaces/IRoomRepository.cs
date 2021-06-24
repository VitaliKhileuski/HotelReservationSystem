using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IRoomRepository : IBaseRepository<RoomEntity>
    {
        IEnumerable<RoomEntity> GetRoomsPageFromHotel(int pageNumber, int pageSize, int hotelId);
        Task<int> GetRoomsCount(int hotelId);
    }
}