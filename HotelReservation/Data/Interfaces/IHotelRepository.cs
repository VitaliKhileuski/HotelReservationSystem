using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IHotelRepository : IBaseRepository<HotelEntity>
    {
        IEnumerable<HotelEntity> GetHotelAdminsHotels(int pageNumber, int pageSize, UserEntity hotelAdmin);
        Task<int> GetHotelAdminsHotelsCount(UserEntity hotelAdmin);
    }
}