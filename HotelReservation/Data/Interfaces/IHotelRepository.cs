using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservation.Data.Entities;

namespace HotelReservation.Data.Interfaces
{
    public interface IHotelRepository : IBaseRepository<HotelEntity>
    {
        IEnumerable<HotelEntity> GetHotelAdminsHotels(int pageNumber, int pageSize, UserEntity hotelAdmin);
        Task<int> GetHotelAdminsHotelsCount(UserEntity hotelAdmin);
         IEnumerable<string> GetHotelNames();

         public IEnumerable<HotelEntity> GetFilteredHotels(string country, string city, string hotelName, string email,
             string surname);
    }
}