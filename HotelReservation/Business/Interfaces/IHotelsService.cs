using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IHotelsService
    {
        Task AddHotel(HotelModel hotel);
        Task<HotelModel> GetById(int id);
        Task UpdateHotelAdmin(int hotelId, int userId);
        Task UpdateHotel(int hotelId, HotelModel hotel,int userId);
        Task DeleteHotelById(int hotelId);
        Tuple<List<HotelModel>,int> GetFilteredHotels(DateTime checkInDate, DateTime checkOutDate, string country, string city, Pagination hotelPagination);
        Task<Tuple<IEnumerable<HotelModel>, int>> GetHotelsPage(Pagination hotelPagination);
        Task<ICollection<UserModel>> GetHotelAdmins(int hotelId);
        Task DeleteHotelAdmin(int hotelId, int userId);
        Task<Tuple<IEnumerable<HotelModel>, int>> GetHotelAdminPages(Pagination hotelPagination, int hotelAdminId);
    }
}