using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Models;
using Business.Models.FilterModels;

namespace Business.Interfaces
{
    public interface IHotelsService
    {
        Task AddHotel(HotelModel hotel);
        Task<HotelModel> GetById(Guid id);
        Task UpdateHotelAdmin(Guid hotelId,Guid adminId);
        Task UpdateHotel(Guid hotelId, HotelModel hotel,string userId);
        Task DeleteHotelById(Guid hotelId,string userId);
        Task<PageInfo<HotelModel>> GetFilteredHotels(HotelFilterModel hotelFilter, Pagination hotelPagination,SortModel sortModel);
        Task<ICollection<UserModel>> GetHotelAdmins(Guid hotelId);
        Task DeleteHotelAdmin(Guid hotelId, Guid adminId);
        Task<PageInfo<HotelModel>> GetHotelsPageForHotelAdmin(HotelFilterModel hotelFilter, Pagination hotelPagination, SortModel sortModel);
        IEnumerable<string> GetHotelNames(string hotelName, int limit);
        Task<IEnumerable<string>> GetHotelRoomsNumbers(Guid hotelId,string userId,string roomNumber, int limit);
        Task<ICollection<ServiceModel>> GetHotelServices(Guid hotelId);
    }
}