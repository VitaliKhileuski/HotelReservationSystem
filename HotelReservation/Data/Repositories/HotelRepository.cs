using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;
using HotelReservation.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelReservation.Data.Repositories
{
    public class HotelRepository : BaseRepository<HotelEntity> , IHotelRepository
    {
        private readonly Context _db;

        public HotelRepository(Context context) : base(context)
        {
            _db = context;
        }
        public IEnumerable<HotelEntity> GetHotelAdminsHotels(int pageNumber, int pageSize, UserEntity hotelAdmin)
        {
            return _db.Hotels.Skip((pageNumber - 1) * pageSize).Take(pageSize).Where(x => x.Admins.Contains(hotelAdmin));
        }
        public async Task<int> GetHotelAdminsHotelsCount(UserEntity hotelAdmin)
        {
            return await _db.Hotels.Where(x => x.Admins.Contains(hotelAdmin)).CountAsync();
        }

        public IEnumerable<string> GetHotelNames()
        {
            return _db.Hotels.Select(x => x.Name).Distinct();
        }
    }
}