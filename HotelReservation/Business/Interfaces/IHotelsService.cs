using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;

namespace Business.Interfaces
{
    public interface IHotelsService
    {
        Task AddHotel(HotelModel hotel);
        Task<HotelModel> GetById(Guid id);
        Task UpdateHotelAdmin(Guid hotelId,Guid adminId);
        Task UpdateHotel(Guid hotelId, HotelModel hotel,string userId);
        Task DeleteHotelById(Guid hotelId,string userId);
        Task<PageInfo<HotelModel>> GetFilteredHotels(string userId, DateTime? checkInDate, DateTime? checkOutDate, string country, string city,string hotelName,string email,string surname, Pagination hotelPagination);
        Task<Tuple<IEnumerable<HotelModel>, int>> GetHotelsPage(Pagination hotelPagination);
        Task<ICollection<UserModel>> GetHotelAdmins(Guid hotelId);
        Task DeleteHotelAdmin(Guid hotelId, Guid adminId);
        Task<Tuple<IEnumerable<HotelModel>, int>> GetHotelAdminPages(Pagination hotelPagination, Guid hotelAdminId);

        IEnumerable<string> GetHotelNames();
    }
}