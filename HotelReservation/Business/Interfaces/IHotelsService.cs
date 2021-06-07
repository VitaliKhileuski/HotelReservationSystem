using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IHotelsService
    {
        Task AddHotel(HotelModel hotel,int HotelAdminId);
        Task<HotelModel> GetById(int id);
        Task UpdateHotelAdmin(int hotelId, int userId);
        Task UpdateHotel(int hotelId, HotelModel hotel,int userId);
        Task DeleteHotelById(int hotelId);
        Tuple<List<HotelModel>,int> GetFilteredHotels(DateTime checkInDate, DateTime checkOutDate, string country, string city, HotelPagination hotelPagination);
        Task<Tuple<IEnumerable<HotelModel>, int>> GetHotelsPage(HotelPagination hotelPagination);
    }
}