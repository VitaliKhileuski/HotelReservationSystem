using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Helpers;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class RoomRepository : BaseRepository<RoomEntity>, IRoomRepository
    {
        private readonly Context _db;

        public RoomRepository(Context context) : base(context)
        {
            _db = context;
        }
        public IEnumerable<RoomEntity> GetRoomsPageFromHotel(int pageNumber, int pageSize,Guid hotelId)
        {
            return _db.Rooms.Skip((pageNumber - 1) * pageSize).Take(pageSize).Where(x => x.HotelId==hotelId);
        }
        public async Task<int> GetRoomsCount(Guid hotelId)
        {
            return await _db.Rooms.Where(x => x.HotelId==hotelId).CountAsync();
        }

        public IEnumerable<RoomEntity> GetFilteredRooms(HotelEntity hotel,string roomNumber,string sortField,bool ascending)
        {
            var filteredRooms = hotel.Rooms.Where(x =>
                !string.IsNullOrEmpty(roomNumber) && x.RoomNumber.StartsWith(roomNumber) || string.IsNullOrEmpty(roomNumber));
            return string.IsNullOrEmpty(sortField) ? filteredRooms : filteredRooms.AsQueryable().OrderByPropertyName(sortField,ascending);
        }
    }
}
