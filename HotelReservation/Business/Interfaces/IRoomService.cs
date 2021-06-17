using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;
using HotelReservation.Data.Entities;

namespace Business.Interfaces
{
    public interface IRoomService
    {
        Task AddRoom(int hotelId, RoomModel room, int userId); 
        Task<ICollection<RoomModel>> GetRoomsFromHotel(int hotelId);
        Task UpdateRoom(int roomId, int userId, RoomModel room);
        Task DeleteRoom(int roomId, int userId);
        Task<PageInfo<RoomModel>> GetRoomsPage(int hotelId, Pagination hotelPagination);

    }
}